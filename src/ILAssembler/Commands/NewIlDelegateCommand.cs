using System;
using System.ComponentModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Reflection;
using System.Reflection.Emit;

namespace ILAssembler.Commands
{
    [Cmdlet(VerbsCommon.New, "IlDelegate", DefaultParameterSetName = ByAstSignatureSet)]
    [Alias("il")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class NewIlDelegateCommand : PSCmdlet
    {
        private const string ByAstSignatureSet = "ByAstSignature";

        private const string ByDelegateSignatureSet = "ByDelegateSignature";

        [Hidden, EditorBrowsable(EditorBrowsableState.Never)]
        public NewIlDelegateCommand()
        {
        }

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ByAstSignatureSet)]
        [ValidateNotNull]
        public ScriptBlock? Signature { get; set; }

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ByDelegateSignatureSet)]
        [ValidateNotNull]
        public Type? DelegateType { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        [ValidateNotNull]
        public ScriptBlock? Body { get; set; }

        protected override void BeginProcessing()
        {
            DynamicMethod? method;
            try
            {
                method = CreateDynamicMethod();
                if (method is null)
                {
                    return;
                }

                CilAssembler.CompileTo(
                    (ScriptBlockAst)Body!.Ast,
                    method);
            }
            catch (ILParseException ilParseException)
            {
                WriteError(
                    new ErrorRecord(
                        ilParseException,
                        ilParseException.Errors?.FirstOrDefault()?.ErrorId ?? "ILParseException",
                        ErrorCategory.InvalidArgument,
                        null));
                return;
            }

            Debug.Assert(DelegateType is not null, "DelegateType should not be null if method is not null.");
            try
            {
                WriteObject(method.CreateDelegate(DelegateType));
            }
            catch (BadImageFormatException badImageException)
            {
                WriteError(
                    new ErrorRecord(
                        new PSArgumentException(SR.BadImageFormat, badImageException),
                        nameof(SR.BadImageFormat),
                        ErrorCategory.InvalidData,
                        method));
            }
        }

        private DynamicMethod? CreateDynamicMethod()
        {
            if (DelegateType is not null)
            {
                if (!DelegateType.IsSubclassOf(typeof(Delegate)))
                {
                    WriteError(
                        new ErrorRecord(
                            new PSArgumentException(SR.InvalidDelegateType, nameof(DelegateType)),
                            nameof(SR.InvalidDelegateType),
                            ErrorCategory.InvalidArgument,
                            DelegateType));
                    return null;
                }

                MethodInfo? invokeMethod = DelegateType.GetMethod(nameof(Action.Invoke));
                if (invokeMethod is null)
                {
                    WriteError(
                        new ErrorRecord(
                            new PSArgumentException(SR.DelegateTypeMissingInvoke, nameof(DelegateType)),
                            nameof(SR.DelegateTypeMissingInvoke),
                            ErrorCategory.InvalidArgument,
                            DelegateType));
                    return null;
                }

                ParameterInfo[] parameters = invokeMethod.GetParameters();
                var parameterTypes = new Type[parameters.Length];
                for (int i = parameterTypes.Length - 1; i >= 0; i--)
                {
                    parameterTypes[i] = parameters[i].ParameterType;
                }

                return new DynamicMethod(
                    invokeMethod.Name,
                    invokeMethod.ReturnType,
                    parameterTypes,
                    typeof(NewIlDelegateCommand).Module,
                    skipVisibility: true);
            }

            var signatureParser = new MethodSignatureParser(
                rejectCtor: true,
                requireResolvableDeclaringType: false);

            Signature!.Ast.Visit(signatureParser);
            var signature = (MethodIdentifier)signatureParser.GetMemberIdentifier(Signature!.Ast.Extent);

            DelegateType = DelegateTypeFactory.GetDelegateType(signature);

            return new DynamicMethod(
                signature.Name,
                signature.ReturnType.GetModifiedType(),
                signature.Parameters.ToModifiedTypeArray(),
                typeof(NewIlDelegateCommand).Module,
                skipVisibility: true);
        }
    }
}

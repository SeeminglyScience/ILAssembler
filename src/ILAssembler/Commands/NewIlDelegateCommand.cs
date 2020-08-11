using System;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Reflection.Emit;

namespace ILAssembler.Commands
{
    [Cmdlet(VerbsCommon.New, "IlDelegate", DefaultParameterSetName = ByAstSignatureSet)]
    [Alias("il")]
    public sealed class NewIlDelegateCommand : PSCmdlet
    {
        private const string ByAstSignatureSet = "ByAstSignature";

        private const string ByDelegateSignatureSet = "ByDelegateSignature";

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
            DynamicMethod method;
            try
            {
                if (DelegateType is not null)
                {
                    if (!DelegateType.IsSubclassOf(typeof(Delegate)))
                    {
                        ThrowTerminatingError(
                            new ErrorRecord(
                                new PSArgumentException(
                                    "The specified delegate type does not inherit from \"System.Delegate\".",
                                    nameof(DelegateType)),
                                "InvalidDelegateType",
                                ErrorCategory.InvalidArgument,
                                DelegateType));
                        return;
                    }

                    var invokeMethod = DelegateType.GetMethod(nameof(Action.Invoke));
                    if (invokeMethod is null)
                    {
                        ThrowTerminatingError(
                            new ErrorRecord(
                                new PSArgumentException(
                                    "The specified delegate type does not have an usable \"Invoke\" method.",
                                    nameof(DelegateType)),
                                "InvalidDelegateType",
                                ErrorCategory.InvalidArgument,
                                DelegateType));
                        return;
                    }

                    method = new DynamicMethod(
                        invokeMethod.Name,
                        invokeMethod.ReturnType,
                        invokeMethod.GetParameters().Select(p => p.ParameterType).ToArray(),
                        restrictedSkipVisibility: true);
                }
                else
                {
                    var signatureParser = new MethodSignatureParser(
                        rejectCtor: true,
                        requireResolvableDeclaringType: false);

                    Signature!.Ast.Visit(signatureParser);
                    var signature = (MethodIdentifier)signatureParser.GetMemberIdentifier(Signature!.Ast.Extent);

                    DelegateType = DelegateTypeFactory.GetDelegateType(
                        signature,
                        shouldCreateDelegate: false,
                        Signature.Ast.Extent);

                    method = new DynamicMethod(
                        signature.Name,
                        signature.ReturnType.GetModifiedType(),
                        signature.Parameters.ToModifiedTypeArray(),
                        restrictedSkipVisibility: true);
                }

                CilAssembler.CompileTo(
                    (ScriptBlockAst)Body!.Ast,
                    method);
            }
            catch (ILParseException ilParseException)
            {
                ThrowTerminatingError(
                    new ErrorRecord(
                        ilParseException,
                        "ILParseException",
                        ErrorCategory.InvalidArgument,
                        null));
                return;
            }

            WriteObject(method.CreateDelegate(DelegateType));
        }
    }
}

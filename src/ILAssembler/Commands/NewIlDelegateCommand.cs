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
            if (SessionState.LanguageMode is not PSLanguageMode.FullLanguage)
            {
                WriteError(
                    new ErrorRecord(
                        new PSInvalidOperationException(SR.FullLanguageRequired),
                        nameof(SR.FullLanguageRequired),
                        ErrorCategory.InvalidOperation,
                        targetObject: null));
                return;
            }

            Debug.Assert(Body is not null, "Engine should ensure body is not null.");
            try
            {
                if (Signature is not null)
                {
                    WriteObject(
                        CilAssemblage.CreateDelegate(
                            (ScriptBlockAst)Signature.Ast,
                            (ScriptBlockAst)Body.Ast),
                        enumerateCollection: false);
                    return;
                }

                Debug.Assert(
                    DelegateType is not null,
                    "Engine should ensure either Signature or DelegateType are not null.");

                WriteObject(
                    CilAssemblage.CreateDelegate(
                        DelegateType,
                        (ScriptBlockAst)Body.Ast));
            }
            catch (ILParseException ilParseException)
            {
                WriteError(
                    new ErrorRecord(
                        ilParseException,
                        ilParseException.Errors?.FirstOrDefault()?.ErrorId ?? "ILParseException",
                        ErrorCategory.InvalidArgument,
                        null));
            }
            catch (InvalidOperationException invalidOperation)
            {
                object? data = invalidOperation.Data[typeof(BranchBuilder)];
                if (data is not null)
                {
                    (_, int id) = (ValueTuple<string?, int>)data;
                    WriteError(
                        new ErrorRecord(
                            invalidOperation,
                            nameof(SR.LabelNotMarked),
                            ErrorCategory.InvalidOperation,
                            targetObject: id));
                    return;
                }

                throw;
            }
            catch (ArgumentException argumentException)
            {
                if (argumentException.TryCreateErrorRecord(out ErrorRecord record))
                {
                    WriteError(record);
                    return;
                }

                throw;
            }
            catch (BadImageFormatException badImageException)
            {
                WriteError(
                    new ErrorRecord(
                        new PSArgumentException(SR.BadImageFormat, badImageException),
                        nameof(SR.BadImageFormat),
                        ErrorCategory.InvalidData,
                        targetObject: null));
            }
        }
    }
}

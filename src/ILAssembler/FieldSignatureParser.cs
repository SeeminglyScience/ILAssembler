using System.Management.Automation.Language;

namespace ILAssembler
{
    internal class FieldSignatureParser : MemberSignatureParser
    {
        public override MemberIdentifier GetMemberIdentifier(IScriptExtent subject)
        {
            if (DeclaringType is null)
            {
                Throw.ParseException(subject, nameof(SR.MissingDeclaringType), SR.MissingDeclaringType);
            }

            if (ReturnType is null)
            {
                Throw.ParseException(subject, nameof(SR.FieldTypeNotFound), SR.FieldTypeNotFound);
            }

            if (Name is null)
            {
                Throw.ParseException(subject, nameof(SR.FieldNameNotFound), SR.FieldNameNotFound);
            }

            var result = new FieldIdentifier(
                DeclaringType,
                Name,
                IsStatic,
                ReturnType);

            Reset();
            return result;
        }

        public override void VisitInvokeMemberExpression(InvokeMemberExpressionAst invokeMemberExpressionAst)
        {
            ErrorUnexpectedNode(invokeMemberExpressionAst);
        }

        protected override ILParseException ErrorExpectedSignature(IScriptExtent extent)
        {
            return ILParseException.Create(
                extent,
                nameof(SR.ExpectedFieldSignature),
                SR.ExpectedFieldSignature);
        }
    }
}

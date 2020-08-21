using System.Management.Automation.Language;

namespace ILAssembler
{
    internal class FieldSignatureParser : MemberSignatureParser
    {
        public override MemberIdentifier GetMemberIdentifier(IScriptExtent subject)
        {
            if (DeclaringType is null)
            {
                throw Error.MissingDeclaringType(subject);
            }

            if (ReturnType is null)
            {
                throw Error.Parse(
                    subject,
                    nameof(Strings.FieldTypeNotFound),
                    Strings.FieldTypeNotFound);
            }

            if (Name is null)
            {
                throw Error.Parse(
                    subject,
                    nameof(Strings.FieldNameNotFound),
                    Strings.FieldNameNotFound);
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
            return Error.Parse(
                extent,
                nameof(Strings.ExpectedFieldSignature),
                Strings.ExpectedFieldSignature);
        }
    }
}

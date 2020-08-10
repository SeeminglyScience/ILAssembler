using System.Management.Automation.Language;

namespace ILAssembler
{
    internal class FieldSignatureParser : MemberSignatureParser
    {
        public override MemberIdentifier GetMemberIdentifier(IScriptExtent subject)
        {

            if (DeclaringType is null)
            {
                throw subject.GetParseError(
                    "DeclaringTypeNotFound",
                    "Unable to determine declaring type.");
            }

            if (ReturnType is null)
            {
                throw subject.GetParseError(
                    "FieldTypeNotFound",
                    "Unable to determine field type.");
            }

            if (Name is null)
            {
                throw subject.GetParseError(
                    "FieldNameNotFound",
                    "Unable to determine field name.");
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
            return extent.GetParseError(
                "ExpectedFieldSignature",
                "Expected field signature declaration, e.g. [FieldType] [DeclaringType]._fieldName");
        }
    }
}

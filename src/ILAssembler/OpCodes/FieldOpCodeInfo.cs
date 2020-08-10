using System.Management.Automation.Language;
using System.Reflection.Metadata;

namespace ILAssembler.OpCodes
{
    internal class FieldOpCodeInfo : TokenOpCodeInfo
    {
        public FieldOpCodeInfo(string name, ILOpCode opCode) : base(name, opCode)
        {
        }

        protected override string ExpectedSignatureErrorId => "ExpectedFieldSignature";

        protected override string ExpectedSignatureMessage => "Expected field signature, e.g. { [FieldType] [DeclaringType]._fieldName }";

        protected override int GetToken(CilAssemblyContext context, ScriptBlockExpressionAst signatureBody)
        {
            var parser = new FieldSignatureParser();
            signatureBody.ScriptBlock.Visit(parser);
            MemberIdentifier identifier = parser.GetMemberIdentifierAndReset(signatureBody.Extent);
            return identifier.GetToken(context, signatureBody.Extent);
        }
    }
}

using System.Management.Automation.Language;
using System.Reflection.Metadata;

namespace ILAssembler.OpCodes
{
    internal class MethodOpCodeInfo : TokenOpCodeInfo
    {
        public MethodOpCodeInfo(string name, ILOpCode opCode) : base(name, opCode)
        {
        }

        protected override string ExpectedSignatureErrorId => "ExpectedMethodSignature";

        protected override string ExpectedSignatureMessage => "Expected method signature, e.g. { [ReturnType] [DeclaringType].MethodName([type] $optionalParameterName) }";

        protected override int GetToken(CilAssemblyContext context, ScriptBlockExpressionAst signatureBody)
        {
            var parser = new MethodSignatureParser(rejectCtor: true, requireResolvableDeclaringType: true);
            signatureBody.ScriptBlock.Visit(parser);
            MemberIdentifier identifier = parser.GetMemberIdentifierAndReset(signatureBody.Extent);
            return identifier.GetToken(context, signatureBody.Extent);
        }
    }
}

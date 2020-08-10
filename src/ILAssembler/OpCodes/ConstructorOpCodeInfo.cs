using System.Management.Automation.Language;
using System.Reflection.Metadata;

namespace ILAssembler.OpCodes
{
    internal class ConstructorOpCodeInfo : TokenOpCodeInfo
    {
        public ConstructorOpCodeInfo(string name, ILOpCode opCode) : base(name, opCode)
        {
        }

        protected override string ExpectedSignatureErrorId => "ExpectedConstructorSignature";

        protected override string ExpectedSignatureMessage => "Expected constructor signature, e.g. { [void] [DeclaringType].new([type] $optionalParameterName) }";

        protected override int GetToken(CilAssemblyContext context, ScriptBlockExpressionAst signatureBody)
        {
            var parser = new MethodSignatureParser(rejectCtor: false, requireResolvableDeclaringType: true);
            signatureBody.ScriptBlock.Visit(parser);
            MemberIdentifier identifier = parser.GetMemberIdentifierAndReset(signatureBody.Extent);
            if (identifier is not ConstructorIdentifier)
            {
                throw ErrorExpectedSignature(signatureBody);
            }

            return identifier.GetToken(context, signatureBody.Extent);
        }
    }
}

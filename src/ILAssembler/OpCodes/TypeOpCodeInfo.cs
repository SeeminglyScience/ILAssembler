using System.Management.Automation.Language;
using System.Reflection.Metadata;

namespace ILAssembler.OpCodes
{
    internal class TypeOpCodeInfo : TokenOpCodeInfo
    {
        public TypeOpCodeInfo(string name, ILOpCode opCode) : base(name, opCode)
        {
        }

        protected override string ExpectedSignatureErrorId => "ExpectedTypeSignature";

        protected override string ExpectedSignatureMessage => "Expected type signature, e.g. { [string] }";

        protected override int GetToken(CilAssemblyContext context, ScriptBlockExpressionAst signatureBody)
        {
            var parser = new TypeArgumentSignatureParser(allowPinned: false, allowByRef: true);
            signatureBody.ScriptBlock.Visit(parser);
            TypedIdentifier identifier = parser.GetSignatureAndReset(signatureBody.Extent);
            return context.ILInfo.GetTokenFor(identifier.GetModifiedType().TypeHandle);
        }
    }
}

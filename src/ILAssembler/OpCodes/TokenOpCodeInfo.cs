using System.Management.Automation.Language;
using System.Reflection.Metadata;

namespace ILAssembler.OpCodes
{
    internal abstract class TokenOpCodeInfo : GeneralOpCodeInfo
    {
        protected TokenOpCodeInfo(string name, ILOpCode opCode) : base(name, opCode)
        {
        }

        protected abstract string ExpectedSignatureErrorId { get; }

        protected abstract string ExpectedSignatureMessage { get; }

        public override void Emit(CilAssemblyContext context, CommandAst ast)
        {
            ast.AssertArgumentCount(1);
            if (!(ast.CommandElements[1] is ScriptBlockExpressionAst signatureBody))
            {
                throw ErrorExpectedSignature(ast.CommandElements[1]);
            }

            int token = GetToken(context, signatureBody);
            context.Encoder.OpCode(OpCode);
            EmitImpl(context, token);
        }

        protected virtual ILParseException ErrorExpectedSignature(Ast ast)
        {
            return Error.Parse(
                ast,
                ExpectedSignatureErrorId,
                ExpectedSignatureMessage);
        }

        protected virtual void EmitImpl(CilAssemblyContext context, int token)
        {
            context.Encoder.Token(token);
        }

        protected abstract int GetToken(
            CilAssemblyContext context,
            ScriptBlockExpressionAst signatureBody);

        protected int FallBackGetToken(
            TokenOpCodeInfo fallBackTo,
            CilAssemblyContext context,
            ScriptBlockExpressionAst signatureBody)
        {
            return fallBackTo.GetToken(context, signatureBody);
        }
    }
}

using System.Management.Automation.Language;
using System.Reflection.Metadata;

namespace ILAssembler.OpCodes
{
    internal class EmitOpCodeInfo : OpCodeInfo
    {
        public override string Name => "_emit";

        public override ILOpCode OpCode => default;

        public override void Emit(CilAssemblyContext context, in InstructionArguments arguments)
        {
            arguments.AssertArgumentCount(1);
            arguments[0].Visit(new ConstantEmitter(context));
        }

        private sealed class ConstantEmitter : SignatureParser
        {
            private readonly CilAssemblyContext _context;

            public ConstantEmitter(CilAssemblyContext context) => _context = context;

            public override void VisitParenExpression(ParenExpressionAst parenExpressionAst)
            {
                parenExpressionAst.Pipeline.Visit(this);
            }

            public override void VisitArrayLiteral(ArrayLiteralAst arrayLiteralAst)
            {
                for (int i = 0; i < arrayLiteralAst.Elements.Count; i++)
                {
                    arrayLiteralAst.Elements[i].Visit(this);
                }
            }

            public override void VisitConstantExpression(ConstantExpressionAst constantExpressionAst)
            {
                _context.Encoder.CodeBuilder.WriteByte(constantExpressionAst.ReadNumber<byte>());
            }
        }
    }
}

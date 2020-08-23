using System.Management.Automation.Language;
using System.Reflection.Metadata;

namespace ILAssembler.OpCodes
{
    internal abstract class VariableExpressionOpCodeInfo : OpCodeInfo
    {
        protected readonly ILOpCode IfByte;

        protected readonly ILOpCode IfInt;

        protected VariableExpressionOpCodeInfo(string name, ILOpCode ifByte, ILOpCode ifInt)
        {
            Name = name;
            IfByte = ifByte;
            IfInt = ifInt;
        }

        public override string Name { get; }

        public override ILOpCode OpCode => default;

        public override void Emit(CilAssemblyContext context, CommandAst ast)
        {
            ast.AssertArgumentCount(1);
            if (!(ast.CommandElements[1] is VariableExpressionAst variable))
            {
                throw Error.UnexpectedType(ast.CommandElements[1], "variable");
            }

            var index = GetIndex(context, variable);
            if (index >= 0xFF)
            {
                context.Encoder.OpCode(IfInt);
                context.Encoder.CodeBuilder.WriteCompressedSignedInteger(index);
                return;
            }

            if (TryGetSimpleOpCode(index, out ILOpCode simple))
            {
                context.Encoder.OpCode(simple);
                return;
            }

            context.Encoder.OpCode(IfByte);
            context.Encoder.CodeBuilder.WriteByte((byte)index);
        }

        protected abstract int GetIndex(CilAssemblyContext context, VariableExpressionAst variable);

        protected virtual bool TryGetSimpleOpCode(int index, out ILOpCode opCode)
        {
            opCode = default;
            return false;
        }
    }
}

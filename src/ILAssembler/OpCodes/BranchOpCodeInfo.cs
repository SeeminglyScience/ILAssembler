using System.Management.Automation.Language;
using System.Reflection.Metadata;

namespace ILAssembler.OpCodes
{
    internal sealed class BranchOpCodeInfo : GeneralOpCodeInfo
    {
        public BranchOpCodeInfo(string name, ILOpCode opCode) : base(name, opCode)
        {
        }

        public override void Emit(CilAssemblyContext context, CommandAst ast)
        {
            ast.AssertArgumentCount(1);
            if (ast.CommandElements[1] is StringConstantExpressionAst stringConstant)
            {
                context.Encoder.Branch(
                    OpCode,
                    context.GetOrAddLabel(stringConstant.Value));

                return;
            }

            context.Encoder.OpCode(OpCode);
            if (OpCode.GetBranchOperandSize() == 1)
            {
                sbyte shortArg = ast.CommandElements[1].ReadNumber<sbyte>();
                context.Encoder.CodeBuilder.WriteSByte(shortArg);
                return;
            }

            int longArg = ast.CommandElements[1].ReadNumber<int>();
            context.Encoder.CodeBuilder.WriteCompressedSignedInteger(longArg);
        }
    }
}

using System.Management.Automation.Language;
using System.Reflection.Metadata;

namespace ILAssembler.OpCodes
{
    internal sealed class BranchOpCodeInfo : GeneralOpCodeInfo
    {
        public BranchOpCodeInfo(string name, ILOpCode opCode) : base(name, opCode)
        {
        }

        public override void Emit(CilAssemblyContext context, in InstructionArguments arguments)
        {
            arguments.AssertArgumentCount(1);
            if (arguments[0] is StringConstantExpressionAst stringConstant)
            {
                context.BranchBuilder.Branch(
                    OpCode,
                    context.GetOrAddLabel(stringConstant.Value));

                return;
            }

            context.Encoder.OpCode(OpCode);
            if (OpCode.GetBranchOperandSize() == 1)
            {
                sbyte shortArg = arguments[0].ReadNumber<sbyte>();
                context.Encoder.CodeBuilder.WriteSByte(shortArg);
                return;
            }

            int longArg = arguments[0].ReadNumber<int>();
            context.Encoder.CodeBuilder.WriteCompressedSignedInteger(longArg);
        }
    }
}

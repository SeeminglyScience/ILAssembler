using System.Management.Automation.Language;
using System.Reflection.Metadata;

namespace ILAssembler.OpCodes
{
    internal sealed class NoOperandOpCodeInfo : GeneralOpCodeInfo
    {
        public NoOperandOpCodeInfo(string name, ILOpCode opCode)
            : base(name, opCode)
        {
        }

        public override void Emit(CilAssemblyContext context, in InstructionArguments arguments)
        {
            arguments.AssertArgumentCount(0);
            context.Encoder.OpCode(OpCode);
        }
    }
}

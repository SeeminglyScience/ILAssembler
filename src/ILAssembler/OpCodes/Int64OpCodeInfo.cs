using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace ILAssembler.OpCodes
{
    internal sealed class Int64OpCodeInfo : SingleNumericArgumentOpCodeInfo<long>
    {
        public Int64OpCodeInfo(string name, ILOpCode opCode)
            : base(name, opCode)
        {
        }

        protected override void EmitArgument(ref InstructionEncoder encoder, long argument)
        {
            encoder.CodeBuilder.WriteInt64(argument);
        }
    }
}

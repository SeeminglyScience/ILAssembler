using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace ILAssembler.OpCodes
{
    internal sealed class ByteOpCodeInfo : SingleNumericArgumentOpCodeInfo<byte>
    {
        public ByteOpCodeInfo(string name, ILOpCode opCode)
            : base(name, opCode)
        {
        }

        protected override void EmitArgument(ref InstructionEncoder encoder, byte argument)
        {
            encoder.CodeBuilder.WriteByte(argument);
        }
    }
}

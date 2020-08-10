using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace ILAssembler.OpCodes
{
    internal sealed class Int32OpCodeInfo : SingleNumericArgumentOpCodeInfo<int>
    {
        public Int32OpCodeInfo(string name, ILOpCode opCode)
            : base(name, opCode)
        {
        }

        protected override void EmitArgument(ref InstructionEncoder encoder, int argument)
        {
            encoder.CodeBuilder.WriteCompressedSignedInteger(argument);
        }
    }
}

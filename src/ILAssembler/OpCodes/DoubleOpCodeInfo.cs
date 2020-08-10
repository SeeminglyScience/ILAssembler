using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace ILAssembler.OpCodes
{
    internal sealed class DoubleOpCodeInfo : SingleNumericArgumentOpCodeInfo<double>
    {
        public DoubleOpCodeInfo(string name, ILOpCode opCode)
            : base(name, opCode)
        {
        }

        protected override void EmitArgument(ref InstructionEncoder encoder, double argument)
        {
            encoder.CodeBuilder.WriteDouble(argument);
        }
    }
}

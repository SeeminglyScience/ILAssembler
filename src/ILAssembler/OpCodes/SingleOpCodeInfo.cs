using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace ILAssembler.OpCodes
{
    internal sealed class SingleOpCodeInfo : SingleNumericArgumentOpCodeInfo<float>
    {
        public SingleOpCodeInfo(string name, ILOpCode opCode)
            : base(name, opCode)
        {
        }

        protected override void EmitArgument(ref InstructionEncoder encoder, float argument)
        {
            encoder.CodeBuilder.WriteSingle(argument);
        }
    }
}

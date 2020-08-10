using System.Reflection.Metadata;

namespace ILAssembler.OpCodes
{
    internal abstract class GeneralOpCodeInfo : OpCodeInfo
    {
        protected GeneralOpCodeInfo(string name, ILOpCode opCode)
        {
            Name = name;
            OpCode = opCode;
        }

        public override string Name { get; }

        public override ILOpCode OpCode { get; }
    }
}

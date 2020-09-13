using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace ILAssembler.OpCodes
{
    internal abstract class SingleNumericArgumentOpCodeInfo<T> : GeneralOpCodeInfo
        where T : unmanaged
    {
        protected SingleNumericArgumentOpCodeInfo(string name, ILOpCode opCode)
            : base(name, opCode)
        {
        }

        public override void Emit(CilAssemblyContext context, in InstructionArguments arguments)
        {
            arguments.AssertArgumentCount(1);
            T argument = arguments[0].ReadNumber<T>();
            context.Encoder.OpCode(OpCode);
            EmitArgument(ref context.Encoder, argument);
        }

        protected abstract void EmitArgument(ref InstructionEncoder encoder, T argument);
    }
}

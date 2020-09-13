using System.Management.Automation.Language;
using System.Reflection.Metadata;

namespace ILAssembler.OpCodes
{
    internal abstract class OpCodeInfo
    {
        public abstract string Name { get; }

        public abstract ILOpCode OpCode { get; }

        public abstract void Emit(CilAssemblyContext context, in InstructionArguments arguments);
    }
}

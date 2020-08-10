using System.Management.Automation.Language;
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

        public override void Emit(CilAssemblyContext context, CommandAst ast)
        {
            ast.AssertArgumentCount(1);
            T argument = ast.CommandElements[1].ReadNumber<T>();
            context.Encoder.OpCode(OpCode);
            EmitArgument(ref context.Encoder, argument);
        }

        protected abstract void EmitArgument(ref InstructionEncoder encoder, T argument);
    }
}

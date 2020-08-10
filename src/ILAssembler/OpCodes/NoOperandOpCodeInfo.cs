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

        public override void Emit(CilAssemblyContext context, CommandAst ast)
        {
            ast.AssertArgumentCount(0);
            context.Encoder.OpCode(OpCode);
        }
    }
}

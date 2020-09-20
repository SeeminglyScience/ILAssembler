using System.Management.Automation.Language;
using System.Reflection.Metadata;

namespace ILAssembler.OpCodes
{
    internal class StringOpCodeInfo : GeneralOpCodeInfo
    {
        public StringOpCodeInfo(string name, ILOpCode opCode) : base(name, opCode)
        {
        }

        public override void Emit(CilAssemblyContext context, in InstructionArguments arguments)
        {
            arguments.AssertArgumentCount(1);
            if (arguments[0] is not StringConstantExpressionAst stringConstant)
            {
                Throw.UnexpectedType(arguments[0], "string");
                return;
            }

            int token = context.ILInfo.GetTokenFor(stringConstant.Value);
            context.Encoder.OpCode(OpCode);
            context.Encoder.Token(token);
        }
    }
}

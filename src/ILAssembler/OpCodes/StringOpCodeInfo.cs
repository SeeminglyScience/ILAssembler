using System.Management.Automation.Language;
using System.Reflection.Metadata;

namespace ILAssembler.OpCodes
{
    internal class StringOpCodeInfo : GeneralOpCodeInfo
    {
        public StringOpCodeInfo(string name, ILOpCode opCode) : base(name, opCode)
        {
        }

        public override void Emit(CilAssemblyContext context, CommandAst ast)
        {
            ast.AssertArgumentCount(1);
            if (!(ast.CommandElements[1] is StringConstantExpressionAst stringConstant))
            {
                throw Error.UnexpectedType(ast.CommandElements[1], "string");
            }

            var token = context.ILInfo.GetTokenFor(stringConstant.Value);
            context.Encoder.OpCode(OpCode);
            context.Encoder.Token(token);
        }
    }
}

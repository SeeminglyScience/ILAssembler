using System.Management.Automation.Language;
using System.Reflection.Metadata;

namespace ILAssembler.OpCodes
{
    internal sealed class SwitchOpCodeInfo : GeneralOpCodeInfo
    {
        public SwitchOpCodeInfo(string name, ILOpCode opCode)
            : base(name, opCode)
        {
        }

        public override void Emit(CilAssemblyContext context, CommandAst ast)
        {
            ast.AssertArgumentCount(1);
            ReadOnlyListSegment<ExpressionAst> branches;
            if (ast.CommandElements[1] is ArrayLiteralAst arrayLiteralAst)
            {
                branches = arrayLiteralAst.Elements;
            }
            else if (ast.CommandElements[1] is StringConstantExpressionAst stringConstant)
            {
                branches = new ExpressionAst[] { stringConstant };
            }
            else
            {
                throw ast.GetParseError(
                    "MissingBranches",
                    "Expected an array literal of branch names.");
            }

            var labels = new LabelHandle[branches.Count];
            for (int i = 0; i < labels.Length; i++)
            {
                if (!(branches[i] is StringConstantExpressionAst branchName))
                {
                    throw branches[i].GetParseError(
                        "MissingBranch",
                        "Expected branch name.");
                }

                labels[i] = context.GetOrAddLabel(branchName.Value);
            }

            context.BranchBuilder.Switch(labels);
        }
    }
}

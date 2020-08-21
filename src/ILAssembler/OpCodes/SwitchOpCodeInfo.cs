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
                throw Error.Parse(
                    ast,
                    nameof(Strings.MissingBranches),
                    Strings.MissingBranches);
            }

            var labels = new LabelHandle[branches.Count];
            for (int i = 0; i < labels.Length; i++)
            {
                if (!(branches[i] is StringConstantExpressionAst branchName))
                {
                    throw Error.Parse(
                        branches[i],
                        nameof(Strings.MissingBranchName),
                        Strings.MissingBranchName);
                }

                labels[i] = context.GetOrAddLabel(branchName.Value);
            }

            context.BranchBuilder.Switch(labels);
        }
    }
}

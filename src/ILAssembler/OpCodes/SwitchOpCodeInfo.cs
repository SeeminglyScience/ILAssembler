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

        public override void Emit(CilAssemblyContext context, in InstructionArguments arguments)
        {
            arguments.AssertArgumentCount(1);
            ReadOnlyListSegment<ExpressionAst> branches;
            if (arguments[0] is ArrayLiteralAst arrayLiteralAst)
            {
                branches = arrayLiteralAst.Elements;
            }
            else if (arguments[0] is StringConstantExpressionAst stringConstant)
            {
                branches = new ExpressionAst[] { stringConstant };
            }
            else
            {
                throw Error.Parse(
                    arguments.StartPosition.ToScriptExtent(),
                    nameof(Strings.MissingBranches),
                    Strings.MissingBranches);
            }

            var labels = new LabelHandle[branches.Count];
            for (int i = 0; i < labels.Length; i++)
            {
                if (branches[i] is not StringConstantExpressionAst branchName)
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

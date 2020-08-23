using System.Management.Automation.Language;

namespace ILAssembler
{
    internal class SignatureParser : CilAssemblerBase
    {
        public override void VisitScriptBlock(ScriptBlockAst scriptBlockAst)
        {
            if (scriptBlockAst.ParamBlock is not null)
            {
                throw Error.ElementNotSupported(scriptBlockAst.ParamBlock, "param");
            }

            if (scriptBlockAst.DynamicParamBlock is not null)
            {
                throw Error.ElementNotSupported(scriptBlockAst.DynamicParamBlock, "dynamicparam");
            }

            if (scriptBlockAst.BeginBlock is not null)
            {
                throw Error.ElementNotSupported(scriptBlockAst.BeginBlock, "begin");
            }

            if (scriptBlockAst.ProcessBlock is not null)
            {
                throw Error.ElementNotSupported(scriptBlockAst.ProcessBlock, "process");
            }

            scriptBlockAst.EndBlock.Visit(this);
        }

        public override void VisitNamedBlock(NamedBlockAst namedBlockAst)
        {
            if (namedBlockAst.Statements.Count > 1)
            {
                throw Error.Parse(
                    namedBlockAst.Statements[1],
                    nameof(Strings.InvalidStatementCount),
                    Strings.InvalidStatementCount);
            }

            namedBlockAst.Statements[0].Visit(this);
        }

        public override void VisitPipeline(PipelineAst pipelineAst)
        {
            if (pipelineAst.PipelineElements.Count > 1)
            {
                var extentToThrow = ExtentOps.ExtentOf(
                    pipelineAst.PipelineElements[1].Extent,
                    pipelineAst.PipelineElements[^1].Extent);

                throw Error.ElementNotSupported(extentToThrow, "|");
            }

            pipelineAst.PipelineElements[0].Visit(this);
        }

        public override void VisitCommandExpression(CommandExpressionAst commandExpressionAst)
        {
            commandExpressionAst.Expression.Visit(this);
        }
    }
}

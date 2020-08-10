using System.Management.Automation.Language;

namespace ILAssembler
{
    internal class SignatureParser : CilAssemblerBase
    {
        public override void VisitScriptBlock(ScriptBlockAst scriptBlockAst)
        {
            if (scriptBlockAst.ParamBlock is not null)
            {
                throw scriptBlockAst.ParamBlock.ErrorElementNotSupported("param");
            }

            if (scriptBlockAst.DynamicParamBlock is not null)
            {
                throw scriptBlockAst.DynamicParamBlock.ErrorElementNotSupported("dynamicparam");
            }

            if (scriptBlockAst.BeginBlock is not null)
            {
                throw scriptBlockAst.BeginBlock.ErrorElementNotSupported("begin");
            }

            if (scriptBlockAst.ProcessBlock is not null)
            {
                throw scriptBlockAst.ProcessBlock.ErrorElementNotSupported("process");
            }

            scriptBlockAst.EndBlock.Visit(this);
        }

        public override void VisitNamedBlock(NamedBlockAst namedBlockAst)
        {
            if (namedBlockAst.Statements.Count > 1)
            {
                throw namedBlockAst.Statements[1].GetParseError(
                    "InvalidStatementCount",
                    "Expected only one statement in this signature declaration.");
            }

            namedBlockAst.Statements[0].Visit(this);
        }

        public override void VisitPipeline(PipelineAst pipelineAst)
        {
            if (pipelineAst.PipelineElements.Count > 1)
            {
                var extentToThrow = ExtentOps.ExtentOf(
                    pipelineAst.PipelineElements[1].Extent,
                    pipelineAst.PipelineElements[pipelineAst.PipelineElements.Count - 1].Extent);

                throw extentToThrow.ErrorElementNotSupported("|");
            }

            pipelineAst.PipelineElements[0].Visit(this);
        }

        public override void VisitCommandExpression(CommandExpressionAst commandExpressionAst)
        {
            commandExpressionAst.Expression.Visit(this);
        }
    }
}

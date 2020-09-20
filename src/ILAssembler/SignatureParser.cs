using System.Management.Automation.Language;

namespace ILAssembler
{
    internal class SignatureParser : CilAssemblerBase
    {
        public override void VisitScriptBlock(ScriptBlockAst scriptBlockAst)
        {
            if (scriptBlockAst.ParamBlock is not null)
            {
                Throw.ElementNotSupported(scriptBlockAst.ParamBlock, "param");
            }

            if (scriptBlockAst.DynamicParamBlock is not null)
            {
                Throw.ElementNotSupported(scriptBlockAst.DynamicParamBlock, "dynamicparam");
            }

            if (scriptBlockAst.BeginBlock is not null)
            {
                Throw.ElementNotSupported(scriptBlockAst.BeginBlock, "begin");
            }

            if (scriptBlockAst.ProcessBlock is not null)
            {
                Throw.ElementNotSupported(scriptBlockAst.ProcessBlock, "process");
            }

            scriptBlockAst.EndBlock.Visit(this);
        }

        public override void VisitNamedBlock(NamedBlockAst namedBlockAst)
        {
            if (namedBlockAst.Statements.Count == 0)
            {
                Throw.ParseException(
                    namedBlockAst.Extent,
                    nameof(SR.MissingSignatureBody),
                    SR.MissingSignatureBody);
                return;
            }

            if (namedBlockAst.Statements.Count > 1)
            {
                Throw.ParseException(
                    namedBlockAst.Statements[1].Extent,
                    nameof(SR.InvalidStatementCount),
                    SR.InvalidStatementCount);
                return;
            }

            namedBlockAst.Statements[0].Visit(this);
        }

        public override void VisitPipeline(PipelineAst pipelineAst)
        {
            if (pipelineAst.PipelineElements.Count > 1)
            {
                IScriptExtent extentToThrow = ExtentOps.ExtentOf(
                    pipelineAst.PipelineElements[1].Extent,
                    pipelineAst.PipelineElements[^1].Extent);

                Throw.ElementNotSupported(extentToThrow, "|");
            }

            pipelineAst.PipelineElements[0].Visit(this);
        }

        public override void VisitCommandExpression(CommandExpressionAst commandExpressionAst)
        {
            commandExpressionAst.Expression.Visit(this);
        }
    }
}

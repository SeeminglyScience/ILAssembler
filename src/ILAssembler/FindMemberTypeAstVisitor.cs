using System.Linq;
using System.Management.Automation.Language;

namespace ILAssembler
{
    internal class FindMemberTypeAstVisitor : DefaultCustomAstVisitor
    {
        public override object? VisitScriptBlock(ScriptBlockAst scriptBlockAst)
        {
            return scriptBlockAst.EndBlock?.Visit(this);
        }

        public override object? VisitNamedBlock(NamedBlockAst namedBlockAst)
        {
            return namedBlockAst.Statements?.FirstOrDefault()?.Visit(this);
        }

        public override object? VisitPipeline(PipelineAst pipelineAst)
        {
            return pipelineAst.PipelineElements?.FirstOrDefault()?.Visit(this);
        }

        public override object? VisitCommandExpression(CommandExpressionAst commandExpressionAst)
        {
            return commandExpressionAst.Expression.Visit(this);
        }

        public override object? VisitConvertExpression(ConvertExpressionAst convertExpressionAst)
        {
            return convertExpressionAst.Child.Visit(this);
        }

        public override object? VisitTypeExpression(TypeExpressionAst typeExpressionAst)
        {
            return typeExpressionAst;
        }

        public override object? VisitMemberExpression(MemberExpressionAst memberExpressionAst)
        {
            return memberExpressionAst;
        }

        public override object? VisitInvokeMemberExpression(InvokeMemberExpressionAst invokeMemberExpressionAst)
        {
            return invokeMemberExpressionAst;
        }
    }
}

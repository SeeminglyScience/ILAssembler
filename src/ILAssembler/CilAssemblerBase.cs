using System.Management.Automation.Language;

namespace ILAssembler
{
    internal class CilAssemblerBase : ICustomAstVisitor
    {
        protected virtual void DefaultVisit(Ast ast) => throw ErrorUnexpectedNode(ast);

        protected ILParseException ErrorUnexpectedNode(Ast ast) => ErrorUnexpectedNode(ast.Extent);

        protected virtual ILParseException ErrorUnexpectedNode(IScriptExtent extent)
        {
            return ILParseException.Create(
                extent,
                nameof(SR.UnexpectedNode),
                SR.UnexpectedNode);
        }

        public virtual void VisitArrayExpression(ArrayExpressionAst arrayExpressionAst) => DefaultVisit(arrayExpressionAst);

        public virtual void VisitArrayLiteral(ArrayLiteralAst arrayLiteralAst) => DefaultVisit(arrayLiteralAst);

        public virtual void VisitAssignmentStatement(AssignmentStatementAst assignmentStatementAst) => DefaultVisit(assignmentStatementAst);

        public virtual void VisitAttribute(AttributeAst attributeAst) => DefaultVisit(attributeAst);

        public virtual void VisitAttributedExpression(AttributedExpressionAst attributedExpressionAst) => DefaultVisit(attributedExpressionAst);

        public virtual void VisitBinaryExpression(BinaryExpressionAst binaryExpressionAst) => DefaultVisit(binaryExpressionAst);

        public virtual void VisitBlockStatement(BlockStatementAst blockStatementAst) => DefaultVisit(blockStatementAst);

        public virtual void VisitBreakStatement(BreakStatementAst breakStatementAst) => DefaultVisit(breakStatementAst);

        public virtual void VisitCatchClause(CatchClauseAst catchClauseAst) => DefaultVisit(catchClauseAst);

        public virtual void VisitCommand(CommandAst commandAst) => DefaultVisit(commandAst);

        public virtual void VisitCommandExpression(CommandExpressionAst commandExpressionAst) => DefaultVisit(commandExpressionAst);

        public virtual void VisitCommandParameter(CommandParameterAst commandParameterAst) => DefaultVisit(commandParameterAst);

        public virtual void VisitConstantExpression(ConstantExpressionAst constantExpressionAst) => DefaultVisit(constantExpressionAst);

        public virtual void VisitContinueStatement(ContinueStatementAst continueStatementAst) => DefaultVisit(continueStatementAst);

        public virtual void VisitConvertExpression(ConvertExpressionAst convertExpressionAst) => DefaultVisit(convertExpressionAst);

        public virtual void VisitDataStatement(DataStatementAst dataStatementAst) => DefaultVisit(dataStatementAst);

        public virtual void VisitDoUntilStatement(DoUntilStatementAst doUntilStatementAst) => DefaultVisit(doUntilStatementAst);

        public virtual void VisitDoWhileStatement(DoWhileStatementAst doWhileStatementAst) => DefaultVisit(doWhileStatementAst);

        public virtual void VisitErrorExpression(ErrorExpressionAst errorExpressionAst) => DefaultVisit(errorExpressionAst);

        public virtual void VisitErrorStatement(ErrorStatementAst errorStatementAst) => DefaultVisit(errorStatementAst);

        public virtual void VisitExitStatement(ExitStatementAst exitStatementAst) => DefaultVisit(exitStatementAst);

        public virtual void VisitExpandableStringExpression(ExpandableStringExpressionAst expandableStringExpressionAst) => DefaultVisit(expandableStringExpressionAst);

        public virtual void VisitFileRedirection(FileRedirectionAst fileRedirectionAst) => DefaultVisit(fileRedirectionAst);

        public virtual void VisitForEachStatement(ForEachStatementAst forEachStatementAst) => DefaultVisit(forEachStatementAst);

        public virtual void VisitForStatement(ForStatementAst forStatementAst) => DefaultVisit(forStatementAst);

        public virtual void VisitFunctionDefinition(FunctionDefinitionAst functionDefinitionAst) => DefaultVisit(functionDefinitionAst);

        public virtual void VisitHashtable(HashtableAst hashtableAst) => DefaultVisit(hashtableAst);

        public virtual void VisitIfStatement(IfStatementAst ifStmtAst) => DefaultVisit(ifStmtAst);

        public virtual void VisitIndexExpression(IndexExpressionAst indexExpressionAst) => DefaultVisit(indexExpressionAst);

        public virtual void VisitInvokeMemberExpression(InvokeMemberExpressionAst invokeMemberExpressionAst) => DefaultVisit(invokeMemberExpressionAst);

        public virtual void VisitMemberExpression(MemberExpressionAst memberExpressionAst) => DefaultVisit(memberExpressionAst);

        public virtual void VisitMergingRedirection(MergingRedirectionAst mergingRedirectionAst) => DefaultVisit(mergingRedirectionAst);

        public virtual void VisitNamedAttributeArgument(NamedAttributeArgumentAst namedAttributeArgumentAst) => DefaultVisit(namedAttributeArgumentAst);

        public virtual void VisitNamedBlock(NamedBlockAst namedBlockAst) => DefaultVisit(namedBlockAst);

        public virtual void VisitParamBlock(ParamBlockAst paramBlockAst) => DefaultVisit(paramBlockAst);

        public virtual void VisitParameter(ParameterAst parameterAst) => DefaultVisit(parameterAst);

        public virtual void VisitParenExpression(ParenExpressionAst parenExpressionAst) => DefaultVisit(parenExpressionAst);

        public virtual void VisitPipeline(PipelineAst pipelineAst) => DefaultVisit(pipelineAst);

        public virtual void VisitReturnStatement(ReturnStatementAst returnStatementAst) => DefaultVisit(returnStatementAst);

        public virtual void VisitScriptBlock(ScriptBlockAst scriptBlockAst) => DefaultVisit(scriptBlockAst);

        public virtual void VisitScriptBlockExpression(ScriptBlockExpressionAst scriptBlockExpressionAst) => DefaultVisit(scriptBlockExpressionAst);

        public virtual void VisitStatementBlock(StatementBlockAst statementBlockAst) => DefaultVisit(statementBlockAst);

        public virtual void VisitStringConstantExpression(StringConstantExpressionAst stringConstantExpressionAst) => DefaultVisit(stringConstantExpressionAst);

        public virtual void VisitSubExpression(SubExpressionAst subExpressionAst) => DefaultVisit(subExpressionAst);

        public virtual void VisitSwitchStatement(SwitchStatementAst switchStatementAst) => DefaultVisit(switchStatementAst);

        public virtual void VisitThrowStatement(ThrowStatementAst throwStatementAst) => DefaultVisit(throwStatementAst);

        public virtual void VisitTrap(TrapStatementAst trapStatementAst) => DefaultVisit(trapStatementAst);

        public virtual void VisitTryStatement(TryStatementAst tryStatementAst) => DefaultVisit(tryStatementAst);

        public virtual void VisitTypeConstraint(TypeConstraintAst typeConstraintAst) => DefaultVisit(typeConstraintAst);

        public virtual void VisitTypeExpression(TypeExpressionAst typeExpressionAst) => DefaultVisit(typeExpressionAst);

        public virtual void VisitUnaryExpression(UnaryExpressionAst unaryExpressionAst) => DefaultVisit(unaryExpressionAst);

        public virtual void VisitUsingExpression(UsingExpressionAst usingExpressionAst) => DefaultVisit(usingExpressionAst);

        public virtual void VisitVariableExpression(VariableExpressionAst variableExpressionAst) => DefaultVisit(variableExpressionAst);

        public virtual void VisitWhileStatement(WhileStatementAst whileStatementAst) => DefaultVisit(whileStatementAst);

        object ICustomAstVisitor.VisitArrayExpression(ArrayExpressionAst arrayExpressionAst)
        {
            VisitArrayExpression(arrayExpressionAst);
            return null!;
        }

        object ICustomAstVisitor.VisitArrayLiteral(ArrayLiteralAst arrayLiteralAst)
        {
            VisitArrayLiteral(arrayLiteralAst);
            return null!;
        }

        object ICustomAstVisitor.VisitAssignmentStatement(AssignmentStatementAst assignmentStatementAst)
        {
            VisitAssignmentStatement(assignmentStatementAst);
            return null!;
        }

        object ICustomAstVisitor.VisitAttribute(AttributeAst attributeAst)
        {
            VisitAttribute(attributeAst);
            return null!;
        }

        object ICustomAstVisitor.VisitAttributedExpression(AttributedExpressionAst attributedExpressionAst)
        {
            VisitAttributedExpression(attributedExpressionAst);
            return null!;
        }

        object ICustomAstVisitor.VisitBinaryExpression(BinaryExpressionAst binaryExpressionAst)
        {
            VisitBinaryExpression(binaryExpressionAst);
            return null!;
        }

        object ICustomAstVisitor.VisitBlockStatement(BlockStatementAst blockStatementAst)
        {
            VisitBlockStatement(blockStatementAst);
            return null!;
        }

        object ICustomAstVisitor.VisitBreakStatement(BreakStatementAst breakStatementAst)
        {
            VisitBreakStatement(breakStatementAst);
            return null!;
        }

        object ICustomAstVisitor.VisitCatchClause(CatchClauseAst catchClauseAst)
        {
            VisitCatchClause(catchClauseAst);
            return null!;
        }

        object ICustomAstVisitor.VisitCommand(CommandAst commandAst)
        {
            VisitCommand(commandAst);
            return null!;
        }

        object ICustomAstVisitor.VisitCommandExpression(CommandExpressionAst commandExpressionAst)
        {
            VisitCommandExpression(commandExpressionAst);
            return null!;
        }

        object ICustomAstVisitor.VisitCommandParameter(CommandParameterAst commandParameterAst)
        {
            VisitCommandParameter(commandParameterAst);
            return null!;
        }

        object ICustomAstVisitor.VisitConstantExpression(ConstantExpressionAst constantExpressionAst)
        {
            VisitConstantExpression(constantExpressionAst);
            return null!;
        }

        object ICustomAstVisitor.VisitContinueStatement(ContinueStatementAst continueStatementAst)
        {
            VisitContinueStatement(continueStatementAst);
            return null!;
        }

        object ICustomAstVisitor.VisitConvertExpression(ConvertExpressionAst convertExpressionAst)
        {
            VisitConvertExpression(convertExpressionAst);
            return null!;
        }

        object ICustomAstVisitor.VisitDataStatement(DataStatementAst dataStatementAst)
        {
            VisitDataStatement(dataStatementAst);
            return null!;
        }

        object ICustomAstVisitor.VisitDoUntilStatement(DoUntilStatementAst doUntilStatementAst)
        {
            VisitDoUntilStatement(doUntilStatementAst);
            return null!;
        }

        object ICustomAstVisitor.VisitDoWhileStatement(DoWhileStatementAst doWhileStatementAst)
        {
            VisitDoWhileStatement(doWhileStatementAst);
            return null!;
        }

        object ICustomAstVisitor.VisitErrorExpression(ErrorExpressionAst errorExpressionAst)
        {
            VisitErrorExpression(errorExpressionAst);
            return null!;
        }

        object ICustomAstVisitor.VisitErrorStatement(ErrorStatementAst errorStatementAst)
        {
            VisitErrorStatement(errorStatementAst);
            return null!;
        }

        object ICustomAstVisitor.VisitExitStatement(ExitStatementAst exitStatementAst)
        {
            VisitExitStatement(exitStatementAst);
            return null!;
        }

        object ICustomAstVisitor.VisitExpandableStringExpression(ExpandableStringExpressionAst expandableStringExpressionAst)
        {
            VisitExpandableStringExpression(expandableStringExpressionAst);
            return null!;
        }

        object ICustomAstVisitor.VisitFileRedirection(FileRedirectionAst fileRedirectionAst)
        {
            VisitFileRedirection(fileRedirectionAst);
            return null!;
        }

        object ICustomAstVisitor.VisitForEachStatement(ForEachStatementAst forEachStatementAst)
        {
            VisitForEachStatement(forEachStatementAst);
            return null!;
        }

        object ICustomAstVisitor.VisitForStatement(ForStatementAst forStatementAst)
        {
            VisitForStatement(forStatementAst);
            return null!;
        }

        object ICustomAstVisitor.VisitFunctionDefinition(FunctionDefinitionAst functionDefinitionAst)
        {
            VisitFunctionDefinition(functionDefinitionAst);
            return null!;
        }

        object ICustomAstVisitor.VisitHashtable(HashtableAst hashtableAst)
        {
            VisitHashtable(hashtableAst);
            return null!;
        }

        object ICustomAstVisitor.VisitIfStatement(IfStatementAst ifStmtAst)
        {
            VisitIfStatement(ifStmtAst);
            return null!;
        }

        object ICustomAstVisitor.VisitIndexExpression(IndexExpressionAst indexExpressionAst)
        {
            VisitIndexExpression(indexExpressionAst);
            return null!;
        }

        object ICustomAstVisitor.VisitInvokeMemberExpression(InvokeMemberExpressionAst invokeMemberExpressionAst)
        {
            VisitInvokeMemberExpression(invokeMemberExpressionAst);
            return null!;
        }

        object ICustomAstVisitor.VisitMemberExpression(MemberExpressionAst memberExpressionAst)
        {
            VisitMemberExpression(memberExpressionAst);
            return null!;
        }

        object ICustomAstVisitor.VisitMergingRedirection(MergingRedirectionAst mergingRedirectionAst)
        {
            VisitMergingRedirection(mergingRedirectionAst);
            return null!;
        }

        object ICustomAstVisitor.VisitNamedAttributeArgument(NamedAttributeArgumentAst namedAttributeArgumentAst)
        {
            VisitNamedAttributeArgument(namedAttributeArgumentAst);
            return null!;
        }

        object ICustomAstVisitor.VisitNamedBlock(NamedBlockAst namedBlockAst)
        {
            VisitNamedBlock(namedBlockAst);
            return null!;
        }

        object ICustomAstVisitor.VisitParamBlock(ParamBlockAst paramBlockAst)
        {
            VisitParamBlock(paramBlockAst);
            return null!;
        }

        object ICustomAstVisitor.VisitParameter(ParameterAst parameterAst)
        {
            VisitParameter(parameterAst);
            return null!;
        }

        object ICustomAstVisitor.VisitParenExpression(ParenExpressionAst parenExpressionAst)
        {
            VisitParenExpression(parenExpressionAst);
            return null!;
        }

        object ICustomAstVisitor.VisitPipeline(PipelineAst pipelineAst)
        {
            VisitPipeline(pipelineAst);
            return null!;
        }

        object ICustomAstVisitor.VisitReturnStatement(ReturnStatementAst returnStatementAst)
        {
            VisitReturnStatement(returnStatementAst);
            return null!;
        }

        object ICustomAstVisitor.VisitScriptBlock(ScriptBlockAst scriptBlockAst)
        {
            VisitScriptBlock(scriptBlockAst);
            return null!;
        }

        object ICustomAstVisitor.VisitScriptBlockExpression(ScriptBlockExpressionAst scriptBlockExpressionAst)
        {
            VisitScriptBlockExpression(scriptBlockExpressionAst);
            return null!;
        }

        object ICustomAstVisitor.VisitStatementBlock(StatementBlockAst statementBlockAst)
        {
            VisitStatementBlock(statementBlockAst);
            return null!;
        }

        object ICustomAstVisitor.VisitStringConstantExpression(StringConstantExpressionAst stringConstantExpressionAst)
        {
            VisitStringConstantExpression(stringConstantExpressionAst);
            return null!;
        }

        object ICustomAstVisitor.VisitSubExpression(SubExpressionAst subExpressionAst)
        {
            VisitSubExpression(subExpressionAst);
            return null!;
        }

        object ICustomAstVisitor.VisitSwitchStatement(SwitchStatementAst switchStatementAst)
        {
            VisitSwitchStatement(switchStatementAst);
            return null!;
        }

        object ICustomAstVisitor.VisitThrowStatement(ThrowStatementAst throwStatementAst)
        {
            VisitThrowStatement(throwStatementAst);
            return null!;
        }

        object ICustomAstVisitor.VisitTrap(TrapStatementAst trapStatementAst)
        {
            VisitTrap(trapStatementAst);
            return null!;
        }

        object ICustomAstVisitor.VisitTryStatement(TryStatementAst tryStatementAst)
        {
            VisitTryStatement(tryStatementAst);
            return null!;
        }

        object ICustomAstVisitor.VisitTypeConstraint(TypeConstraintAst typeConstraintAst)
        {
            VisitTypeConstraint(typeConstraintAst);
            return null!;
        }

        object ICustomAstVisitor.VisitTypeExpression(TypeExpressionAst typeExpressionAst)
        {
            VisitTypeExpression(typeExpressionAst);
            return null!;
        }

        object ICustomAstVisitor.VisitUnaryExpression(UnaryExpressionAst unaryExpressionAst)
        {
            VisitUnaryExpression(unaryExpressionAst);
            return null!;
        }

        object ICustomAstVisitor.VisitUsingExpression(UsingExpressionAst usingExpressionAst)
        {
            VisitUsingExpression(usingExpressionAst);
            return null!;
        }

        object ICustomAstVisitor.VisitVariableExpression(VariableExpressionAst variableExpressionAst)
        {
            VisitVariableExpression(variableExpressionAst);
            return null!;
        }

        object ICustomAstVisitor.VisitWhileStatement(WhileStatementAst whileStatementAst)
        {
            VisitWhileStatement(whileStatementAst);
            return null!;
        }
    }
}

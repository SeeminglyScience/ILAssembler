using System.Management.Automation.Language;

namespace ILAssembler
{
    internal abstract class MemberSignatureParser : SignatureParser
    {
        protected string? Name { get; private set; }

        protected TypedIdentifier? DeclaringType { get; private set; }

        protected bool IsStatic { get; private set; }

        protected TypedIdentifier? ReturnType { get; private set; }

        public virtual MemberIdentifier GetMemberIdentifierAndReset(IScriptExtent extent)
        {
            try
            {
                return GetMemberIdentifier(extent);
            }
            finally
            {
                Reset();
            }
        }

        public abstract MemberIdentifier GetMemberIdentifier(IScriptExtent extent);

        public override void VisitCommandExpression(CommandExpressionAst commandExpressionAst)
        {
            if (!(commandExpressionAst.Expression is ConvertExpressionAst convert))
            {
                throw ErrorExpectedSignature(commandExpressionAst.Expression.Extent);
            }

            convert.Visit(new ReturnTypeSignatureParser(this));
        }

        public override void VisitMemberExpression(MemberExpressionAst memberExpressionAst)
        {
            VisitMemberImpl(memberExpressionAst);
        }

        public override void VisitInvokeMemberExpression(InvokeMemberExpressionAst invokeMemberExpressionAst)
        {
            VisitMemberImpl(invokeMemberExpressionAst);
        }

        protected virtual void Reset()
        {
            DeclaringType = null;
            Name = null;
            IsStatic = false;
            ReturnType = null;
        }

        protected virtual void VisitMemberImpl(MemberExpressionAst memberExpressionAst)
        {
            if (memberExpressionAst.Expression is TypeExpressionAst typeExpression)
            {
                DeclaringType = TypeResolver.Resolve(typeExpression.TypeName);
            }
            else
            {
                HandleUnresolvableSubject(memberExpressionAst.Expression);
            }

            if (!(memberExpressionAst.Member is StringConstantExpressionAst stringConstant))
            {
                throw memberExpressionAst.Member.GetParseError(
                    "ExpectedStringConstant",
                    "Member name must be a constant value.");
            }

            Name = stringConstant.Value;
            IsStatic = memberExpressionAst.Static;
        }

        protected virtual void HandleUnresolvableSubject(ExpressionAst ast)
        {
            throw ast.GetParseError(
                "ExpectedTypeExpression",
                "Member expression subject must be a resolvable type expression.");
        }

        protected abstract ILParseException ErrorExpectedSignature(IScriptExtent extent);

        private class ReturnTypeSignatureParser : TypeSignatureParser
        {
            private readonly MemberSignatureParser _parent;

            public ReturnTypeSignatureParser(MemberSignatureParser parent)
                : base(NameExpectation.Reject, allowPinned: false, allowByRef: true)
            {
                _parent = parent;
            }

            public override void VisitMemberExpression(MemberExpressionAst memberExpressionAst)
            {
                _parent.ReturnType = GetSignatureAndReset(
                    memberExpressionAst.Extent.StartScriptPosition.ToScriptExtent());

                memberExpressionAst.Visit(_parent);
            }

            public override void VisitInvokeMemberExpression(InvokeMemberExpressionAst invokeMemberExpressionAst)
            {
                _parent.ReturnType = GetSignatureAndReset(
                    invokeMemberExpressionAst.Extent.StartScriptPosition.ToScriptExtent());

                invokeMemberExpressionAst.Visit(_parent);
            }
        }
    }
}

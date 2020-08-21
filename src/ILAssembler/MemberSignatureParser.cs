using System;
using System.Collections.Generic;
using System.Management.Automation.Language;

namespace ILAssembler
{
    internal abstract class MemberSignatureParser : SignatureParser
    {
        protected string? Name { get; private set; }

        protected TypedIdentifier? DeclaringType { get; private set; }

        protected bool IsStatic { get; set; }

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
                throw Error.Parse(
                    memberExpressionAst.Member,
                    nameof(Strings.ExpectedConstantMemberName),
                    Strings.ExpectedConstantMemberName);
            }

            Name = stringConstant.Value;
            IsStatic = memberExpressionAst.Static;
        }

        protected virtual void VisitAnonymousSignature(ExpressionAst expressionAst)
        {
            throw Error.UnexpectedType(expressionAst, "MemberExpression");
        }

        protected virtual void HandleUnresolvableSubject(ExpressionAst ast)
        {
            throw Error.Parse(
                ast,
                nameof(Strings.ExpectedTypeExpression),
                Strings.ExpectedTypeExpression);
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

            public override void VisitArrayExpression(ArrayExpressionAst arrayExpressionAst)
            {
                _parent.ReturnType = GetSignatureAndReset(
                    arrayExpressionAst.Extent.StartScriptPosition.ToScriptExtent());
                _parent.VisitAnonymousSignature(arrayExpressionAst);
            }

            public override void VisitParenExpression(ParenExpressionAst parenExpressionAst)
            {
                _parent.ReturnType = GetSignatureAndReset(
                    parenExpressionAst.Extent.StartScriptPosition.ToScriptExtent());
                _parent.VisitAnonymousSignature(parenExpressionAst);
            }
        }
    }
}

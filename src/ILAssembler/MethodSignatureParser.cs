using System;
using System.Management.Automation.Language;

namespace ILAssembler
{
    internal class MethodSignatureParser : MemberSignatureParser
    {
        private readonly bool _rejectCtor;

        private readonly bool _requireResolvableDeclaringType;

        private TypedIdentifier[]? _genericArgs;

        private TypedIdentifier[]? _parameters;

        public MethodSignatureParser(bool rejectCtor = false, bool requireResolvableDeclaringType = false)
        {
            _rejectCtor = rejectCtor;
            _requireResolvableDeclaringType = requireResolvableDeclaringType;
        }

        public override MemberIdentifier GetMemberIdentifier(IScriptExtent subject)
        {
            if (ReturnType is null)
            {
                throw subject.GetParseError(
                    "ReturnTypeNotFound",
                    "Unable to determine return type.");
            }

            bool isCtor = !_rejectCtor
                && Name is not null
                && Name.Equals("new", StringComparison.Ordinal)
                && (_genericArgs is null || _genericArgs.Length == 0);

            if (isCtor)
            {
                return new ConstructorIdentifier(
                    DeclaringType,
                    IsStatic,
                    ReturnType,
                    _parameters);
            }

            return new MethodIdentifier(
                DeclaringType,
                Name,
                IsStatic,
                ReturnType,
                _genericArgs,
                _parameters);
        }

        protected override void Reset()
        {
            base.Reset();
            _genericArgs = null;
            _parameters = null;
        }

        public override void VisitMemberExpression(MemberExpressionAst memberExpressionAst)
        {
            throw ErrorUnexpectedNode(memberExpressionAst);
        }

        protected override void VisitMemberImpl(MemberExpressionAst memberExpressionAst)
        {
            base.VisitMemberImpl(memberExpressionAst);
            InvokeMemberExpressionAst invokeMemberExpressionAst = (InvokeMemberExpressionAst)memberExpressionAst;
            if (invokeMemberExpressionAst.Arguments is null || invokeMemberExpressionAst.Arguments.Count == 0)
            {
                _genericArgs = Array.Empty<TypedIdentifier>();
                _parameters = Array.Empty<TypedIdentifier>();
                return;
            }

            int firstArgumentIndex = 0;
            if (invokeMemberExpressionAst.Arguments[0] is TypeExpressionAst maybeGenericArgs
                && maybeGenericArgs.TypeName is GenericTypeName maybeGenericArgsName
                && maybeGenericArgsName.TypeName.Name.Equals("g", StringComparison.Ordinal))
            {
                firstArgumentIndex = 1;
                _genericArgs = new TypedIdentifier[maybeGenericArgsName.GenericArguments.Count];
                for (int i = 0; i < _genericArgs.Length; i++)
                {
                    _genericArgs[i] = TypeResolver.Resolve(maybeGenericArgsName.GenericArguments[i]);
                }
            }
            else
            {
                _genericArgs = Array.Empty<TypedIdentifier>();
            }

            var typeParser = new TypeSignatureParser(NameExpectation.Allow, allowPinned: false, allowByRef: true);
            _parameters = new TypedIdentifier[invokeMemberExpressionAst.Arguments.Count - firstArgumentIndex];
            for (int i = 0; i < _parameters.Length; i++)
            {
                var argumentAst = invokeMemberExpressionAst.Arguments[i + firstArgumentIndex];
                argumentAst.Visit(typeParser);
                _parameters[i] = typeParser.GetSignatureAndReset(argumentAst.Extent);
            }
        }

        protected override void HandleUnresolvableSubject(ExpressionAst ast)
        {
            if (_requireResolvableDeclaringType)
            {
                base.HandleUnresolvableSubject(ast);
            }
        }

        protected override ILParseException ErrorExpectedSignature(IScriptExtent extent)
        {
            return extent.GetParseError(
                "ExpectedMethodSignature",
                "Expected method signature declaration, e.g. [ReturnType] [DeclaringType].MethodName([int] $optionalArgName)");
        }
    }
}

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
                throw Error.Parse(
                    subject,
                    nameof(Strings.ReturnTypeNotFound),
                    Strings.ReturnTypeNotFound);
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
            HandleParameters(invokeMemberExpressionAst.Arguments);
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
            return Error.Parse(
                extent,
                nameof(Strings.ExpectedMethodSignature),
                Strings.ExpectedMethodSignature);
        }

        protected override void VisitAnonymousSignature(ExpressionAst expressionAst)
        {
            HandleUnresolvableSubject(expressionAst);
            IsStatic = true;
            PipelineAst pipe;
            if (expressionAst is ArrayExpressionAst arrayExpressionAst)
            {
                if (arrayExpressionAst.SubExpression.Statements.Count == 0)
                {
                    HandleParameters();
                    return;
                }

                if (arrayExpressionAst.SubExpression.Statements.Count > 1)
                {
                    throw Error.Parse(
                        arrayExpressionAst.SubExpression.Statements[1].Extent.StartScriptPosition.ToScriptExtent(),
                        nameof(Strings.MissingEndParenthesisInExpression),
                        Strings.MissingEndParenthesisInExpression);
                }

                if (arrayExpressionAst.SubExpression.Statements[0] is PipelineAst)
                {
                    pipe = (PipelineAst)arrayExpressionAst.SubExpression.Statements[0];
                }
                else
                {
                    throw Error.ElementNotSupported(arrayExpressionAst.SubExpression.Statements[0]);
                }
            }
            else if (expressionAst is ParenExpressionAst parenExpression)
            {
                if (parenExpression.Pipeline is not PipelineAst)
                {
                    throw Error.ElementNotSupported(parenExpression.Pipeline);
                }

                pipe = (PipelineAst)parenExpression.Pipeline;
            }
            else
            {
                throw Error.ElementNotSupported(expressionAst);
            }

            if (pipe.PipelineElements.Count == 0)
            {
                HandleParameters();
                return;
            }

            if (pipe.PipelineElements.Count > 1)
            {
                throw Error.MissingEndParenthesisInExpression(
                    pipe.PipelineElements[1].Extent.StartScriptPosition.ToScriptExtent());
            }

            if (!(pipe.PipelineElements[0] is CommandExpressionAst commandExpression))
            {
                throw Error.ElementNotSupported(pipe.PipelineElements[0]);
            }

            if (commandExpression.Expression is ArrayLiteralAst arrayLiteral)
            {
                HandleParameters(arrayLiteral.Elements);
                return;
            }

            if (commandExpression.Expression is ConvertExpressionAst
                || commandExpression.Expression is TypeExpressionAst)
            {
                HandleParameters(new[] { commandExpression.Expression });
                return;
            }

            throw Error.ElementNotSupported(commandExpression.Expression);
        }

        private void HandleParameters(ReadOnlyListSegment<ExpressionAst> parameters = default)
        {
            if (parameters.Count == 0)
            {
                _genericArgs = Array.Empty<TypedIdentifier>();
                _parameters = Array.Empty<TypedIdentifier>();
                return;
            }

            if (parameters[0] is TypeExpressionAst maybeGenericArgs
                && maybeGenericArgs.TypeName is GenericTypeName maybeGenericArgsName
                && maybeGenericArgsName.TypeName.Name.Equals("g", StringComparison.Ordinal))
            {
                parameters = parameters[1..];
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
            _parameters = new TypedIdentifier[parameters.Count];
            for (int i = 0; i < _parameters.Length; i++)
            {
                var argumentAst = parameters[i];
                argumentAst.Visit(typeParser);
                _parameters[i] = typeParser.GetSignatureAndReset(argumentAst.Extent);
            }
        }
    }
}

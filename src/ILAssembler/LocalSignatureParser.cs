using System;
using System.Collections.Generic;
using System.Management.Automation.Language;

namespace ILAssembler
{
    internal sealed class LocalSignatureParser : SignatureParser
    {
        private TypeSignatureParser? _signatureParser;

        private HashSet<string>? _usedNames;

        private List<TypedIdentifier>? _identifiers;

        public static TypedIdentifier[] ParseLocals(Ast ast)
        {
            var parser = new LocalSignatureParser();
            ast.Visit(parser);
            return parser.GetLocals();
        }

        public TypedIdentifier[] GetLocals()
        {
            return _identifiers?.ToArray() ?? Array.Empty<TypedIdentifier>();
        }

        public override void VisitNamedBlock(NamedBlockAst namedBlockAst)
        {
            foreach (StatementAst statement in namedBlockAst.Statements)
            {
                statement.Visit(this);
            }
        }

        public override void VisitArrayLiteral(ArrayLiteralAst arrayLiteralAst)
        {
            foreach (ExpressionAst element in arrayLiteralAst.Elements)
            {
                element.Visit(this);
            }
        }

        public override void VisitConvertExpression(ConvertExpressionAst convertExpressionAst)
        {
            _identifiers ??= new List<TypedIdentifier>();
            _signatureParser ??= new TypeSignatureParser(
                NameExpectation.Require,
                allowPinned: true,
                allowByRef: true);

            _usedNames ??= new HashSet<string>();

            convertExpressionAst.Visit(_signatureParser);
            TypedIdentifier identifier = _signatureParser.GetSignatureAndReset(convertExpressionAst.Extent);
            if (!_usedNames.Add(identifier.Name))
            {
                throw Error.Parse(
                    convertExpressionAst,
                    nameof(Strings.LocalAlreadyDefined),
                    Strings.LocalAlreadyDefined,
                    identifier.Name);
            }

            _identifiers.Add(identifier);
        }
    }
}

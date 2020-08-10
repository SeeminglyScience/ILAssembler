using System;
using System.Management.Automation.Language;

namespace ILAssembler
{
    internal sealed class TypeArgumentSignatureParser : SignatureParser
    {
        private readonly TypeSignatureParser _parser;

        public TypeArgumentSignatureParser(bool allowPinned, bool allowByRef)
            => _parser = new(NameExpectation.Reject, allowPinned, allowByRef);

        public TypedIdentifier GetSignatureAndReset(IScriptExtent subject)
        {
            return _parser.GetSignatureAndReset(subject);
        }

        public override void VisitCommandExpression(CommandExpressionAst commandExpressionAst)
        {
            commandExpressionAst.Expression.Visit(_parser);
        }
    }
}

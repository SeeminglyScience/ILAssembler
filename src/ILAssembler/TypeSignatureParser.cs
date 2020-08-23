using System;
using System.Management.Automation;
using System.Management.Automation.Language;

namespace ILAssembler
{
    internal class TypeSignatureParser : CilAssemblerBase
    {
        private readonly NameExpectation _nameKind;

        private readonly bool _allowPinned;

        private readonly bool _allowByRef;

        private bool _isByRef;

        private bool _isPinned;

        private Type? _type;

        private string? _name;

        public TypeSignatureParser(NameExpectation nameKind, bool allowPinned, bool allowByRef)
        {
            _nameKind = nameKind;
            _allowPinned = allowPinned;
            _allowByRef = allowByRef;
        }

        public TypedIdentifier GetSignatureAndReset(IScriptExtent subject)
        {
            if (_type is null)
            {
                throw Error.Parse(
                    subject,
                    nameof(Strings.MissingTypeSpecification),
                    Strings.MissingTypeSpecification);
            }

            if (_name is null && _nameKind == NameExpectation.Require)
            {
                throw Error.Parse(
                    subject,
                    nameof(Strings.MissingSignatureIdentifier),
                    Strings.MissingSignatureIdentifier);
            }

            var result = (_type, _name, _isPinned, _isByRef);
            _type = null;
            _name = null;
            _isByRef = false;
            _isPinned = false;
            return result;
        }

        public override void VisitVariableExpression(VariableExpressionAst variableExpressionAst)
        {
            if (_nameKind == NameExpectation.Reject)
            {
                throw Error.Parse(
                    variableExpressionAst,
                    nameof(Strings.ExpectedTypeExpression),
                    Strings.ExpectedTypeExpression);
            }

            _name = variableExpressionAst.VariablePath.UserPath;
        }

        public override void VisitConvertExpression(ConvertExpressionAst convertExpressionAst)
        {
            ITypeName typeName = convertExpressionAst.Type.TypeName;
            if (typeName.FullName.Equals(SpecialTypes.ByRef, StringComparison.Ordinal))
            {
                if (!_allowByRef)
                {
                    throw Error.Parse(
                        typeName.Extent,
                        nameof(Strings.ByRefNotSupported),
                        Strings.ByRefNotSupported);
                }

                if (_isByRef)
                {
                    throw ErrorDuplicateModifier(typeName.Extent, SpecialTypes.ByRef);
                }

                _isByRef = true;
                convertExpressionAst.Child.Visit(this);
                return;
            }

            if (typeName.FullName.Equals(SpecialTypes.Pinned, StringComparison.Ordinal))
            {
                if (!_allowPinned)
                {
                    throw Error.Parse(
                        typeName.Extent,
                        nameof(Strings.PinnedNotSupported),
                        Strings.PinnedNotSupported);
                }

                if (_isPinned)
                {
                    throw ErrorDuplicateModifier(typeName.Extent, SpecialTypes.Pinned);
                }

                _isPinned = true;
                convertExpressionAst.Child.Visit(this);
                return;
            }

            if (_type is not null)
            {
                throw Error.Parse(
                    convertExpressionAst,
                    nameof(Strings.TypeAlreadySpecified),
                    Strings.TypeAlreadySpecified);
            }

            _type = TypeResolver.Resolve(typeName);

            convertExpressionAst.Child.Visit(this);
        }

        public override void VisitTypeExpression(TypeExpressionAst typeExpressionAst)
        {
            if (_nameKind == NameExpectation.Require)
            {
                throw Error.Parse(
                    typeExpressionAst,
                    nameof(Strings.ExpectedVariableExpression),
                    Strings.ExpectedVariableExpression);
            }

            if (_type is not null)
            {
                throw Error.Parse(
                    typeExpressionAst,
                    nameof(Strings.TypeAlreadySpecified),
                    Strings.TypeAlreadySpecified);
            }

            _type = TypeResolver.Resolve(typeExpressionAst.TypeName);
        }

        private static ParseException ErrorDuplicateModifier(IScriptExtent extent, string modifierName)
        {
            return Error.Parse(
                extent,
                nameof(Strings.LocalsModAlreadySpecified),
                Strings.LocalsModAlreadySpecified,
                modifierName);
        }
    }
}

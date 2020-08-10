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
                throw subject.GetParseError(
                    "MissingType",
                    "Missing type declaration.");
            }

            if (_name is null && _nameKind == NameExpectation.Require)
            {
                throw subject.GetParseError(
                    "MissingSignatureName",
                    "Missing variable name.");
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
                throw variableExpressionAst.GetParseError(
                    "ExpectedTypeExpression",
                    "Expected type expression but found variable expression.");
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
                    throw typeName.Extent.GetParseError(
                        "ByRefNotSupported",
                        "ByRef types are not supported in this signature kind.");
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
                    throw typeName.Extent.GetParseError(
                        "PinnedNotSupported",
                        "Pinned types are not supported in this signature kind.");
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
                throw convertExpressionAst.GetParseError(
                    "TypeAlreadySpecified",
                    "The type must be specified only once.");
            }

            _type = TypeResolver.Resolve(typeName);

            convertExpressionAst.Child.Visit(this);
        }

        public override void VisitTypeExpression(TypeExpressionAst typeExpressionAst)
        {
            if (_nameKind == NameExpectation.Require)
            {
                throw typeExpressionAst.GetParseError(
                    "ExpectedVariableExpression",
                    "Expected variable expression but found type expression.");
            }

            if (_type is not null)
            {
                throw typeExpressionAst.GetParseError(
                    "TypeAlreadySpecified",
                    "The type must be specified only once.");
            }

            _type = TypeResolver.Resolve(typeExpressionAst.TypeName);
        }

        private static ParseException ErrorDuplicateModifier(IScriptExtent extent, string modifierName)
        {
            return extent.GetParseError(
                "DuplicateLocalModifier",
                "The modifier \"{0}\" must be specified only once.",
                modifierName);
        }
    }
}

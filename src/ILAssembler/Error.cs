using System.Globalization;
using System.Management.Automation.Language;
using System.Runtime.ExceptionServices;

namespace ILAssembler
{
    internal static class Error
    {
        public static ILParseException MissingEndParenthesisInExpression(IScriptExtent subject)
        {
            return Parse(
                subject,
                nameof(Strings.MissingEndParenthesisInExpression),
                Strings.MissingEndParenthesisInExpression);
        }

        public static ILParseException MissingDeclaringType(IScriptExtent subject)
        {
            return Parse(
                subject,
                nameof(Strings.MissingDeclaringType),
                Strings.MissingDeclaringType);
        }

        public static ILParseException IncompleteCalli(IScriptExtent subject)
        {
            return Parse(
                subject,
                nameof(Strings.IncompleteCalli),
                Strings.IncompleteCalli);
        }

        public static ILParseException ElementNotSupported(Ast ast)
        {
            return ElementNotSupported(
                ast.Extent,
                ast.GetType().Name);
        }

        public static ILParseException ElementNotSupported(Ast ast, string displayName)
        {
            return ElementNotSupported(ast.Extent, displayName);
        }

        public static ILParseException ElementNotSupported(IScriptExtent extent, string displayName)
        {
            return Parse(
                extent,
                nameof(Strings.UnsupportedElement),
                Strings.UnsupportedElement,
                displayName);
        }

        public static ILParseException UnexpectedType(Ast ast, string expectedType)
        {
            return Parse(
                ast,
                nameof(Strings.InvalidArgumentType),
                Strings.InvalidArgumentType,
                expectedType);
        }

        public static ILParseException Parse(Ast ast, string errorId, string message)
            => Parse(ast.Extent, errorId, message);

        public static ILParseException Parse(Ast ast, string errorId, string format, object arg)
            => Parse(ast.Extent, errorId, string.Format(CultureInfo.CurrentCulture, format, arg));

        public static ILParseException Parse(Ast ast, string errorId, string format, object arg0, object arg1)
            => Parse(ast.Extent, errorId, string.Format(CultureInfo.CurrentCulture, format, arg0, arg1));

        public static ILParseException Parse(IScriptExtent extent, string errorId, string format, object arg)
            => Parse(extent, errorId, string.Format(CultureInfo.CurrentCulture, format, arg));

        public static ILParseException Parse(IScriptExtent extent, string errorId, string format, object arg0, object arg1)
            => Parse(extent, errorId, string.Format(CultureInfo.CurrentCulture, format, arg0, arg1));

        public static ILParseException Parse(IScriptExtent extent, string errorId, string message)
        {
            return new ILParseException(
                new[]
                {
                    new ParseError(
                        extent,
                        errorId,
                        message)
                });
        }

        public static ILParseException Parse(
            IScriptExtent extent,
            string errorId,
            string message,
            ExceptionDispatchInfo sourceExceptionInfo)
        {
            return new ILParseException(
                new[]
                {
                    new ParseError(
                        extent,
                        errorId,
                        message)
                },
                sourceExceptionInfo);
        }

        public static ILParseException Parse(
            IScriptExtent extent,
            string errorId,
            ExceptionDispatchInfo sourceExceptionInfo)
        {
            return new ILParseException(
                new[]
                {
                    new ParseError(
                        extent,
                        errorId,
                        sourceExceptionInfo.SourceException.Message)
                },
                sourceExceptionInfo);
        }
    }
}

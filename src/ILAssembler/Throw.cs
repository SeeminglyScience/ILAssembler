using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Management.Automation.Language;
using System.Runtime.CompilerServices;

namespace ILAssembler
{
    internal static class Throw
    {
        [DoesNotReturn, MethodImpl(MethodImplOptions.NoInlining)]
        public static void UnexpectedType(Ast ast, string expectedType)
        {
            throw ILParseException.Create(
                ast.Extent,
                nameof(SR.InvalidArgumentType),
                SR.Format(SR.InvalidArgumentType, expectedType));
        }

        [DoesNotReturn, MethodImpl(MethodImplOptions.NoInlining)]
        public static void ElementNotSupported(Ast ast)
        {
            ElementNotSupported(
                ast.Extent,
                ast.GetType().Name);
        }

        [DoesNotReturn, MethodImpl(MethodImplOptions.NoInlining)]
        public static void ElementNotSupported(Ast ast, string displayName)
        {
            ElementNotSupported(ast.Extent, displayName);
        }

        [DoesNotReturn, MethodImpl(MethodImplOptions.NoInlining)]
        public static void ElementNotSupported(IScriptExtent extent, string displayName)
        {
            throw ILParseException.Create(
                extent,
                nameof(SR.UnsupportedElement),
                SR.Format(SR.UnsupportedElement, displayName));
        }

        [DoesNotReturn, MethodImpl(MethodImplOptions.NoInlining)]
        public static void UnexpectedToken(IScriptExtent extent)
        {
            throw ILParseException.Create(
                extent,
                nameof(SR.UnexpectedToken),
                SR.Format(SR.UnexpectedToken, extent.Text));
        }

        [DoesNotReturn, MethodImpl(MethodImplOptions.NoInlining)]
        public static void CannotReadCommandName(IScriptExtent extent)
        {
            throw ILParseException.Create(
                extent,
                nameof(SR.CannotReadCommandName),
                SR.CannotReadCommandName);
        }

        [DoesNotReturn, MethodImpl(MethodImplOptions.NoInlining)]
        public static void MissingTryStatementBlock(IScriptExtent extent)
        {
            ParseException(extent, nameof(SR.MissingTryStatementBlock), SR.MissingTryStatementBlock);
        }

        [DoesNotReturn, MethodImpl(MethodImplOptions.NoInlining)]
        public static void InvalidOperationException(string message)
        {
            throw new InvalidOperationException(message);
        }

        [DoesNotReturn, MethodImpl(MethodImplOptions.NoInlining)]
        public static void ParseException(IScriptExtent extent, string errorId, string message)
        {
            throw new ILParseException(
                new[]
                {
                    new ParseError(
                        extent,
                        errorId,
                        message)
                });
        }
    }
}

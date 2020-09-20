using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Management.Automation.Language;
using System.Runtime.CompilerServices;

namespace ILAssembler
{
    internal static class Throw
    {
        [DoesNotReturn]
        public static void CannotReadCommandName(IScriptExtent extent)
        {
            throw Error.Parse(extent, nameof(Strings.CannotReadCommandName), Strings.CannotReadCommandName);
        }

        [DoesNotReturn]
        public static void MissingTryStatementBlock(IScriptExtent extent)
        {
            throw Error.Parse(extent, nameof(Strings.MissingTryStatementBlock), Strings.MissingTryStatementBlock);
        }

        [DoesNotReturn, MethodImpl(MethodImplOptions.NoInlining)]
        public static void ParseException(IScriptExtent extent, string errorId, string format, object arg0)
            => ParseException(extent, errorId, string.Format(CultureInfo.CurrentCulture, format, arg0));

        [DoesNotReturn, MethodImpl(MethodImplOptions.NoInlining)]
        public static void ParseException(IScriptExtent extent, string errorId, string format, object arg0, object arg1)
            => ParseException(extent, errorId, string.Format(CultureInfo.CurrentCulture, format, arg0, arg1));

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

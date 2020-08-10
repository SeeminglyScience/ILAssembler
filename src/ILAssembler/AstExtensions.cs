using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Runtime.ExceptionServices;

namespace ILAssembler
{
    internal static class AstExtensions
    {
        private static readonly Dictionary<Type, string> s_ilAsmTypeName = new()
        {
            { typeof(sbyte), "int8" },
            { typeof(short), "int16" },
            { typeof(int), "int32" },
            { typeof(long), "int64" },
            { typeof(byte), "unsigned int8" },
            { typeof(ushort), "unsigned int16" },
            { typeof(uint), "unsigned int32" },
            { typeof(ulong), "unsigned int64" },
        };

        public static ILParseException GetParseError(this Ast ast, string errorId, string message)
            => GetParseError(ast.Extent, errorId, message);

        public static ILParseException GetParseError(this Ast ast, string errorId, string format, object arg)
            => GetParseError(ast.Extent, errorId, string.Format(CultureInfo.CurrentCulture, format, arg));

        public static ILParseException GetParseError(this Ast ast, string errorId, string format, object arg0, object arg1)
            => GetParseError(ast.Extent, errorId, string.Format(CultureInfo.CurrentCulture, format, arg0, arg1));

        public static ILParseException GetParseError(this IScriptExtent extent, string errorId, string format, object arg)
            => GetParseError(extent, errorId, string.Format(CultureInfo.CurrentCulture, format, arg));

        public static ILParseException GetParseError(this IScriptExtent extent, string errorId, string format, object arg0, object arg1)
            => GetParseError(extent, errorId, string.Format(CultureInfo.CurrentCulture, format, arg0, arg1));

        public static ILParseException GetParseError(this IScriptExtent extent, string errorId, string message)
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

        public static ILParseException GetParseError(
            this IScriptExtent extent,
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

        public static ILParseException GetParseError(
            this IScriptExtent extent,
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

        public static T ReadNumber<T>(this CommandElementAst element) where T : unmanaged
        {
            Debug.Assert(typeof(T).IsPrimitive);
            if (!(element is ConstantExpressionAst constant))
            {
                throw element.ErrorUnexpectedType(GetILAsmName(typeof(T)));
            }

            try
            {
                return LanguagePrimitives.ConvertTo<T>(constant.Value);
            }
            catch (Exception e)
            {
                throw element.GetParseError("InvalidArgumentValue", e.Message);
            }
        }

        public static ParseException ErrorUnexpectedType(this Ast ast, string expectedType)
        {
            return GetParseError(
                ast,
                "InvalidArgumentType",
                "Expected value to be of type \"{0}\".",
                expectedType);
        }

        public static ParseException ErrorElementNotSupported(this Ast ast, string displayName)
        {
            return ErrorElementNotSupported(ast.Extent, displayName);
        }

        public static ParseException ErrorElementNotSupported(this IScriptExtent extent, string displayName)
        {
            return GetParseError(
                extent,
                "UnsupportedElement",
                "Language element \"{0}\" is not supported in this context.",
                displayName);
        }

        public static void AssertArgumentCount(this CommandAst command, int expectedCount)
        {
            int actualCount = command.CommandElements.Count - 1;
            if (actualCount == expectedCount)
            {
                return;
            }

            IScriptExtent extentToThrow;
            if (actualCount > expectedCount)
            {
                extentToThrow = command.CommandElements[expectedCount + 1].Extent;
            }
            else
            {
                IScriptPosition endOfLastElement = command.CommandElements[actualCount].Extent.EndScriptPosition;

                var sp = new ScriptPosition(
                    endOfLastElement.File,
                    endOfLastElement.LineNumber,
                    endOfLastElement.Offset,
                    endOfLastElement.GetFullScript());
                extentToThrow = new ScriptExtent(sp, sp);
            }

            throw extentToThrow.GetParseError(
                "UnexpectedArgument",
                "Expected {0} arguments for \"{1}\".",
                expectedCount,
                command.GetCommandName().ToLowerInvariant());
        }

        public static IScriptExtent ToScriptExtent(this IScriptPosition position)
        {
            var newPosition = position.ToConcretePosition();
            return new ScriptExtent(newPosition, newPosition);
        }

        public static ScriptPosition ToConcretePosition(this IScriptPosition position)
        {
            if (position is ScriptPosition concrete)
            {
                return concrete;
            }

            return new ScriptPosition(
                position.File,
                position.LineNumber,
                position.ColumnNumber,
                position.Line,
                position.GetFullScript());
        }

        private static string GetILAsmName(Type type)
        {
            return s_ilAsmTypeName[type];
        }
    }
}

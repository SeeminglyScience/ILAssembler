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

        public static T ReadNumber<T>(this CommandElementAst element) where T : unmanaged
        {
            Debug.Assert(typeof(T).IsPrimitive);
            if (!(element is ConstantExpressionAst constant))
            {
                throw Error.UnexpectedType(element, GetILAsmName(typeof(T)));
            }

            try
            {
                return LanguagePrimitives.ConvertTo<T>(constant.Value);
            }
            catch (Exception e)
            {
                throw Error.Parse(element, "InvalidArgumentValue", e.Message);
            }
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

            throw Error.Parse(
                extentToThrow,
                nameof(Strings.UnexpectedArgument),
                Strings.UnexpectedArgument,
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

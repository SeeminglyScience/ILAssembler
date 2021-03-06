using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            if (element is not ConstantExpressionAst constant)
            {
                Throw.UnexpectedType(element, GetILAsmName(typeof(T)));
                return default;
            }

            try
            {
                return LanguagePrimitives.ConvertTo<T>(constant.Value);
            }
            catch (Exception e)
            {
                throw ILParseException.Create(
                    element.Extent,
                    "InvalidArgumentValue",
                    ExceptionDispatchInfo.Capture(e));
            }
        }

        public static InstructionArguments GetInstructionArguments(this CommandAst command)
        {
            CommandElementAst nameAst = command.CommandElements[0];
            if (nameAst is StringConstantExpressionAst
                and { Value: { Length: > 0 }, StringConstantType: StringConstantType.BareWord })
            {
                return new InstructionArguments(
                    command.CommandElements.Slice(1),
                    nameAst.Extent.EndScriptPosition,
                    nameAst.Extent);
            }

            Throw.CannotReadCommandName(nameAst.Extent);
            // Unreachable.
            return default;
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

            throw ILParseException.Create(
                extentToThrow,
                nameof(SR.UnexpectedArgument),
                SR.Format(SR.UnexpectedArgument, expectedCount, command.GetCommandName().ToLowerInvariant()));
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

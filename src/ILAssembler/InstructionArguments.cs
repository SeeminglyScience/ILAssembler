using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Management.Automation.Language;

namespace ILAssembler
{
    internal readonly struct InstructionArguments : IReadOnlyList<CommandElementAst>
    {
        internal readonly IScriptPosition StartPosition;

        internal readonly string CommandName;

        private readonly ReadOnlyListSegment<CommandElementAst> _list;

        internal InstructionArguments(
            ReadOnlyListSegment<CommandElementAst> list,
            IScriptPosition startPosition,
            string commandName)
        {
            _list = list;
            StartPosition = startPosition;
            CommandName = commandName;
        }

        public CommandElementAst this[int index] => _list[index];

        public int Count => _list.Count;

        public void AssertArgumentCount(int expectedCount)
        {
            if (_list.Count == expectedCount)
            {
                return;
            }

            ThrowUnexpectedArgument(expectedCount);
        }

        internal InstructionArguments Shift()
        {
            Debug.Assert(Count > 0);

            if (_list[0] is StringConstantExpressionAst stringConstant && !string.IsNullOrEmpty(stringConstant.Value))
            {
                return Slice(1, commandName: stringConstant.Value);
            }

            Throw.CannotReadCommandName(_list[0].Extent);
            // Unreachable.
            return default;
        }

        internal InstructionArguments Slice(int start, int? length = null, string? commandName = null)
        {
            IScriptPosition newStartPosition;
            if (start is 0)
            {
                newStartPosition = StartPosition;
            }
            else
            {
                newStartPosition = _list[start - 1].Extent.EndScriptPosition;
            }

            return new InstructionArguments(
                _list.Slice(
                    start,
                    length ?? (_list.Count - start)),
                newStartPosition,
                commandName ?? CommandName);
        }

        public ReadOnlyListSegment<CommandElementAst>.Enumerator GetEnumerator()
            => new ReadOnlyListSegment<CommandElementAst>.Enumerator(_list);

        IEnumerator<CommandElementAst> IEnumerable<CommandElementAst>.GetEnumerator()
        {
            return ((IEnumerable<CommandElementAst>)_list).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_list).GetEnumerator();
        }

        [DoesNotReturn]
        private void ThrowUnexpectedArgument(int expectedCount)
        {
            IScriptExtent extentToThrow;
            if (_list.Count > expectedCount)
            {
                extentToThrow = _list[expectedCount + 1].Extent;
            }
            else
            {
                extentToThrow = StartPosition.ToScriptExtent();
            }

            throw Error.Parse(
                extentToThrow,
                nameof(Strings.UnexpectedArgument),
                Strings.UnexpectedArgument,
                expectedCount,
                CommandName);
        }
    }
}

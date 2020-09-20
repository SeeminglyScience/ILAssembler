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

        internal readonly IScriptExtent NameExtent;

        internal string CommandName => NameExtent.Text;

        private readonly ReadOnlyListSegment<CommandElementAst> _list;

        internal InstructionArguments(
            ReadOnlyListSegment<CommandElementAst> list,
            IScriptPosition startPosition,
            IScriptExtent nameExtent)
        {
            _list = list;
            StartPosition = startPosition;
            NameExtent = nameExtent;
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

            if (_list[0] is StringConstantExpressionAst stringConstant
                and { Value: { Length: > 0 }, StringConstantType: StringConstantType.BareWord })
            {
                return Slice(1, commandName: stringConstant.Extent);
            }

            Throw.CannotReadCommandName(_list[0].Extent);
            // Unreachable.
            return default;
        }

        internal InstructionArguments Slice(int start, int? length = null, IScriptExtent? commandName = null)
        {
            IScriptPosition newStartPosition = start is 0
                ? StartPosition
                : _list[start - 1].Extent.EndScriptPosition;

            return new InstructionArguments(
                _list.Slice(
                    start,
                    length ?? (_list.Count - start)),
                newStartPosition,
                commandName ?? NameExtent);
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

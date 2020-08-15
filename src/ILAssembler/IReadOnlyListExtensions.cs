using System.Collections.Generic;

namespace ILAssembler
{
    internal static class IReadOnlyListExtensions
    {
        public static ReadOnlyListSegment<T> Slice<T>(this IReadOnlyList<T> source, int start)
        {
            return new ReadOnlyListSegment<T>(
                source,
                start,
                source.Count - start);
        }

        public static ReadOnlyListSegment<T> Slice<T>(this IReadOnlyList<T> source, int start, int length)
        {
            return new ReadOnlyListSegment<T>(
                source,
                start,
                length);
        }
    }
}

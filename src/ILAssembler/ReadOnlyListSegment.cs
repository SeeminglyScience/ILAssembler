using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ILAssembler
{
    internal readonly struct ReadOnlyListSegment<T> : IReadOnlyList<T>, IEquatable<ReadOnlyListSegment<T>>
    {
        private readonly IReadOnlyList<T> _source;

        private readonly int _offset;

        public ReadOnlyListSegment(IReadOnlyList<T> source, int offset, int length)
        {
            _source = source;
            _offset = offset;
            Count = length;
        }

        public IReadOnlyList<T> Source => _source ?? Array.Empty<T>();

        public T this[int index] => Source[index + _offset];

        public int Count { get; }

        public static bool operator ==(ReadOnlyListSegment<T> left, ReadOnlyListSegment<T> right)
            => left.Equals(right);

        public static bool operator !=(ReadOnlyListSegment<T> left, ReadOnlyListSegment<T> right)
            => !left.Equals(right);

        public static implicit operator ReadOnlyListSegment<T>(ReadOnlyCollection<T> source)
        {
            return new ReadOnlyListSegment<T>(source, 0, source?.Count ?? 0);
        }

        public static implicit operator ReadOnlyListSegment<T>(T[] source)
        {
            return new ReadOnlyListSegment<T>(source, 0, source?.Length ?? 0);
        }

        public readonly ReadOnlyListSegment<T> Slice(int start)
        {
            return new ReadOnlyListSegment<T>(
                _source,
                _offset + start,
                Count - start);
        }

        public readonly ReadOnlyListSegment<T> Slice(int start, int length)
        {
            return new ReadOnlyListSegment<T>(
                _source,
                _offset + start,
                length);
        }

        public readonly T[] ToArray()
        {
            if (Count == 0)
            {
                return Array.Empty<T>();
            }

            var result = new T[Count];
            if (_source is IList<T> list)
            {
                list.CopyTo(result, _offset);
                return result;
            }

            for (int i = Count - 1; i >= 0; i--)
            {
                result[i] = this[i];
            }

            return result;
        }

        public readonly Enumerator GetEnumerator() => new Enumerator(this);

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool Equals(ReadOnlyListSegment<T> other)
        {
            return Source == other.Source
                && _offset == other._offset
                && Count == other.Count;
        }

        public override bool Equals(object? obj)
        {
            return obj is ReadOnlyListSegment<T> other && Equals(other);
        }

        public override int GetHashCode() => HashCode.Combine(_source, _offset, Count);

        internal struct Enumerator : IEnumerator<T>
        {
            private readonly ReadOnlyListSegment<T> _segment;

            private int _index;

            public Enumerator(ReadOnlyListSegment<T> segment)
            {
                _segment = segment;
                _index = -1;
            }

            public T Current => _segment[_index];

            object? IEnumerator.Current => Current;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (_index == -2)
                {
                    return false;
                }

                if (_index == -1)
                {
                    if (_segment.Count == 0)
                    {
                        return false;
                    }

                    _index++;
                    return true;
                }

                if (_index >= _segment.Count - 1)
                {
                    _index = -2;
                    return false;
                }

                _index++;
                return true;
            }

            public void Reset() => _index = -1;
        }
    }
}

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ILAssembler
{
    internal struct ValueList<T>
    {
        private T[]? _list;

        public int Length => _list?.Length ?? 0;

        public ref T Add()
        {
            if (_list is null)
            {
                _list = new T[1];
                return ref _list[0];
            }

            Array.Resize(ref _list, _list.Length + 1);
            return ref _list[^1];
        }

        public ref T this[int index]
        {
            get
            {
                if (_list is null)
                {
                    ThrowInvalidOperation();
                }

                return ref _list[index];

                [DoesNotReturn, MethodImpl(MethodImplOptions.NoInlining)]
                static void ThrowInvalidOperation()
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}

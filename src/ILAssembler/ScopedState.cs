using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ILAssembler
{
    internal class ScopedState<TState> where TState : struct
    {
        private Stack<TState>? _stack;

        public TState Current;

        public void PushScope()
        {
            (_stack ??= new Stack<TState>()).Push(Current);
            Current = default;
        }

        public ref TState PopScope()
        {
            if (_stack is null or { Count: 0 })
            {
                ThrowCannotPop();
            }

            Current = _stack.Pop();
            return ref Current;

            [DoesNotReturn]
            static void ThrowCannotPop()
            {
                throw new InvalidOperationException("Stack empty.");
            }
        }

        public ScopeHandle UseNewScope()
        {
            PushScope();
            return new ScopeHandle(this);
        }

        public readonly ref struct ScopeHandle
        {
            private readonly ScopedState<TState> _parent;

            public ScopeHandle(ScopedState<TState> parent) => _parent = parent;

            public void Dispose() => _parent.PopScope();
        }
    }
}

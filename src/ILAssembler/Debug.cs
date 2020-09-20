using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using Dbg = System.Diagnostics.Debug;

namespace ILAssembler
{
    internal static class Debug
    {
        [Conditional("DEBUG")]
        public static void Assert([DoesNotReturnIf(false)] bool condition)
        {
            Dbg.Assert(condition);
        }

        [Conditional("DEBUG")]
        public static void Assert([DoesNotReturnIf(false)] bool condition, string? message)
        {
            Dbg.Assert(condition, message);
        }
    }
}

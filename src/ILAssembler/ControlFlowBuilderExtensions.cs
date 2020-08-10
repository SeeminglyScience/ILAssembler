using System;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace ILAssembler
{
    internal static class ControlFlowBuilderExtensions
    {
        private static Action<ControlFlowBuilder, BlobBuilder, BlobBuilder>? s_copyCodeAndFixupBranches;

        private static Action<ControlFlowBuilder, BlobBuilder>? s_serializeExceptionTable;

        public static void CopyCodeAndFixupBranches(
            this ControlFlowBuilder @this,
            BlobBuilder srcBuilder,
            BlobBuilder dstBuilder)
        {
            var action = s_copyCodeAndFixupBranches ??= CreateCopyCodeAndFixupBranchesDelegate();
            action(@this, srcBuilder, dstBuilder);
        }

        public static void SerializeExceptionTable(this ControlFlowBuilder @this, BlobBuilder builder)
        {
            var action = s_serializeExceptionTable ??= CreateSerializeExceptionTableDelegate();
            action(@this, builder);
        }

        private static Action<ControlFlowBuilder, BlobBuilder, BlobBuilder> CreateCopyCodeAndFixupBranchesDelegate()
        {
            var method = typeof(ControlFlowBuilder).GetMethod(
                nameof(CopyCodeAndFixupBranches),
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                binder: null,
                new[] { typeof(BlobBuilder), typeof(BlobBuilder) },
                modifiers: null);

            if (method is null)
            {
                throw new PlatformNotSupportedException(
                    "This version of System.Reflection.Metadata is not supported.");
            }

            return (Action<ControlFlowBuilder, BlobBuilder, BlobBuilder>)Delegate.CreateDelegate(
                typeof(Action<ControlFlowBuilder, BlobBuilder, BlobBuilder>),
                firstArgument: null,
                method);
        }

        private static Action<ControlFlowBuilder, BlobBuilder> CreateSerializeExceptionTableDelegate()
        {
            var method = typeof(ControlFlowBuilder).GetMethod(
                nameof(SerializeExceptionTable),
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                binder: null,
                new[] { typeof(BlobBuilder) },
                modifiers: null);

            if (method is null)
            {
                throw new PlatformNotSupportedException(
                    "This version of System.Reflection.Metadata is not supported.");
            }

            return (Action<ControlFlowBuilder, BlobBuilder>)Delegate.CreateDelegate(
                typeof(Action<ControlFlowBuilder, BlobBuilder>),
                firstArgument: null,
                method);
        }
    }
}

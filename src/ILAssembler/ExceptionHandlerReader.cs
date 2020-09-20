using System.Diagnostics.CodeAnalysis;
using System.Management.Automation.Language;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ILAssembler
{
    internal class ExceptionHandlerReader
    {
        private const string TryKeyword = ".try";

        private const string CatchKeyword = "catch";

        private const string FilterKeyword = "filter";

        private const string FaultKeyword = "fault";

        private const string FinallyKeyword = "finally";

        private readonly CilAssembler _assembler;

        private readonly CilAssemblyContext _context;

        private readonly ScopedState<ReaderState> _state;

        public ExceptionHandlerReader(CilAssembler assembler, CilAssemblyContext context)
        {
            _assembler = assembler;
            _context = context;
            _state = new ScopedState<ReaderState>();
        }

        private ref ReaderState State => ref _state.Current;

        internal bool TryProcessExceptionHandler(in InstructionArguments arguments)
        {
            if (State.Expecting is not Expectation.None || arguments.CommandName is TryKeyword)
            {
                return TryProcessExceptionHandlerImpl(in arguments);
            }

            return false;
        }

        internal void MaybeFinalizePendingRegions(IScriptExtent statementStart)
        {
            if (State.Expecting is Expectation.None)
            {
                return;
            }

            FinalizePendingRegions(statementStart);
        }

        private bool TryProcessExceptionHandlerImpl(in InstructionArguments arguments)
        {
            if (State.Expecting == Expectation.None)
            {
                ProcessTry(arguments);
                return true;
            }

            if (arguments.CommandName is CatchKeyword)
            {
                ProcessCatch(in arguments);
                return true;
            }

            if (arguments.CommandName is FilterKeyword)
            {
                ProcessFilter(in arguments);
                return true;
            }

            if (arguments.CommandName is FaultKeyword)
            {
                ProcessFaultOrFinally(in arguments, isFault: true);
                return true;
            }

            if (arguments.CommandName is FinallyKeyword)
            {
                ProcessFaultOrFinally(in arguments, isFault: false);
                return true;
            }

            FinalizePendingRegions(arguments.NameExtent);
            return false;
        }

        private void FinalizePendingRegions(IScriptExtent statementStart)
        {
            if (State.Expecting is Expectation.AnyHandler)
            {
                throw Error.Parse(
                    statementStart.StartScriptPosition.ToScriptExtent(),
                    nameof(Strings.MissingExceptionHandler),
                    Strings.MissingExceptionHandler);
            }

            if (State.Expecting == Expectation.UntypedCatch)
            {
                throw Error.Parse(
                    statementStart.StartScriptPosition.ToScriptExtent(),
                    nameof(Strings.MissingCatchForFilter),
                    Strings.MissingCatchForFilter);
            }

            for (int i = 0; i < State.Handlers.Length; i++)
            {
                ref Catch handler = ref State.Handlers[i];
                if (handler.IsFilter)
                {
                    _context.BranchBuilder.AddExceptionRegion(
                        ExceptionRegionKind.Filter,
                        State.Try.Start,
                        State.Try.End,
                        handler.Body.Start,
                        handler.Body.End,
                        handler.Trigger.FilterRegion.Start);
                    continue;
                }

                _context.BranchBuilder.AddExceptionRegion(
                    ExceptionRegionKind.Catch,
                    State.Try.Start,
                    State.Try.End,
                    handler.Body.Start,
                    handler.Body.End,
                    catchType: handler.Trigger.EntityToken is 0
                        ? default
                        : MetadataTokens.EntityHandle(handler.Trigger.EntityToken));
            }

            State.Expecting = Expectation.None;
        }

        private void ProcessFaultOrFinally(in InstructionArguments arguments, bool isFault)
        {
            if (State.Expecting is not Expectation.AnyHandler)
            {
                throw Error.Parse(
                    arguments.NameExtent,
                    nameof(Strings.MixedExceptionHandlerTypes),
                    Strings.MixedExceptionHandlerTypes);
            }

            if (arguments.Count == 0 || arguments[0] is not ScriptBlockExpressionAst body)
            {
                ThrowMissingExceptionHandlerBody(
                    arguments.StartPosition.ToScriptExtent(),
                    arguments.CommandName);
                return;
            }

            Region region = ProcessBody(body);
            _context.BranchBuilder.AddExceptionRegion(
                isFault ? ExceptionRegionKind.Fault : ExceptionRegionKind.Finally,
                State.Try.Start,
                State.Try.End,
                region.Start,
                region.End);

            State.Try = default;
            State.Expecting = Expectation.None;
        }

        private void ProcessFilter(in InstructionArguments arguments)
        {
            if (State.Expecting is Expectation.UntypedCatch)
            {
                throw Error.Parse(
                    arguments.StartPosition.ToScriptExtent(),
                    nameof(Strings.MissingCatchForFilter),
                    Strings.MissingCatchForFilter);
            }

            if (arguments.Count == 0 || arguments[0] is not ScriptBlockExpressionAst body)
            {
                ThrowMissingExceptionHandlerBody(
                    arguments.StartPosition.ToScriptExtent(),
                    FilterKeyword);
                return;
            }

            if (arguments.Count > 1)
            {
                ThrowUnexpectedToken(arguments[1].Extent);
                return;
            }

            Region region = ProcessBody(body);
            ref Catch handler = ref State.Handlers.Add();
            handler.IsFilter = true;
            handler.Trigger.FilterRegion = region;
            State.Expecting = Expectation.UntypedCatch;
        }

        private void ProcessCatch(in InstructionArguments arguments)
        {
            if (State.Expecting is Expectation.None)
            {
                ThrowMissingTryBlock(arguments.NameExtent);
            }

            if (arguments.Count == 0 || arguments[0] is not ScriptBlockExpressionAst)
            {
                ThrowMissingCatchBody(arguments.StartPosition.ToScriptExtent());
                return;
            }

            Region handlerRegion;
            if (arguments.Count == 2)
            {
                if (State.Expecting is Expectation.UntypedCatch)
                {
                    ThrowUntypedCatchRequired(arguments[0].Extent);
                    return;
                }

                if (arguments[1] is not ScriptBlockExpressionAst)
                {
                    ThrowMissingCatchBody(arguments[1].Extent.StartScriptPosition.ToScriptExtent());
                    return;
                }

                handlerRegion = ProcessBody((ScriptBlockExpressionAst)arguments[1]);
                ref Catch handler = ref State.Handlers.Add();
                var parser = new TypeArgumentSignatureParser(
                    allowPinned: false,
                    allowByRef: false);

                ((ScriptBlockExpressionAst)arguments[0]).ScriptBlock.Visit(parser);
                TypedIdentifier type = parser.GetSignatureAndReset(arguments[1].Extent);
                handler.Trigger.EntityToken = _context.ILInfo.GetTokenFor(type.Type.TypeHandle);
                handler.Body = handlerRegion;
                State.Expecting = Expectation.FilterOrCatch;
                return;
            }

            if (arguments.Count > 2)
            {
                ThrowUnexpectedToken(arguments[2].Extent);
                return;
            }

            if (State.Expecting is not Expectation.UntypedCatch)
            {
                ThrowMissingCatchBody(arguments.StartPosition.ToScriptExtent());
                return;
            }

            handlerRegion = ProcessBody((ScriptBlockExpressionAst)arguments[0]);
            State.Handlers[^1].Body = handlerRegion;
            State.Expecting = Expectation.FilterOrCatch;

            [DoesNotReturn, MethodImpl(MethodImplOptions.NoInlining)]
            static void ThrowUntypedCatchRequired(IScriptExtent extent)
            {
                throw Error.Parse(
                    extent,
                    nameof(Strings.UntypedCatchRequired),
                    Strings.UntypedCatchRequired);
            }
        }

        [DoesNotReturn, MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowMissingCatchBody(IScriptExtent extent)
        {
            throw Error.Parse(
                extent,
                nameof(Strings.MissingCatchBody),
                Strings.MissingCatchBody);
        }

        [DoesNotReturn, MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowMissingExceptionHandlerBody(IScriptExtent extent, string kind)
        {
            throw Error.Parse(
                extent,
                nameof(Strings.MissingExceptionHandlerBody),
                Strings.MissingExceptionHandlerBody,
                kind);
        }

        [DoesNotReturn, MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowUnexpectedToken(IScriptExtent extent)
        {
            throw Error.Parse(
                extent,
                nameof(Strings.UnexpectedToken),
                Strings.UnexpectedToken,
                extent.Text);
        }

        [DoesNotReturn, MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowMissingTryBlock(IScriptExtent extent)
        {
            throw Error.Parse(
                extent,
                nameof(Strings.MissingPrecedingTry),
                Strings.MissingPrecedingTry);
        }

        private Region ProcessBody(ScriptBlockExpressionAst body)
        {
            LabelHandle start = _context.BranchBuilder.DefineLabel();
            LabelHandle end = _context.BranchBuilder.DefineLabel();

            _context.BranchBuilder.MarkLabel(start);
            using (_state.UseNewScope())
            {
                body.ScriptBlock.Visit(_assembler);
            }
            _context.BranchBuilder.MarkLabel(end);
            return new Region(start, end);
        }

        private void ProcessTry(in InstructionArguments arguments)
        {
            if (arguments.Count == 0 || arguments[0] is not ScriptBlockExpressionAst body)
            {
                Throw.MissingTryStatementBlock(arguments.StartPosition.ToScriptExtent());
                return;
            }

            LabelHandle start = _context.BranchBuilder.DefineLabel();
            LabelHandle end = _context.BranchBuilder.DefineLabel();
            _context.BranchBuilder.MarkLabel(start);

            using (_state.UseNewScope())
            {
                body.ScriptBlock.Visit(_assembler);
            }

            _context.BranchBuilder.MarkLabel(end);
            State.Expecting = Expectation.AnyHandler;
            State.Try = new Region(start, end);
        }

        private struct ReaderState
        {
            public Region Try;

            public Expectation Expecting;

            public ValueList<Catch> Handlers;
        }

        private enum Expectation
        {
            None = 0,

            FilterOrCatch = 1,

            UntypedCatch = 2,

            AnyHandler = 3,
        }

        private struct Catch
        {
            public bool IsFilter;

            public TypeOrFilter Trigger;

            public Region Body;

            [StructLayout(LayoutKind.Explicit)]
            public struct TypeOrFilter
            {
                [FieldOffset(0)]
                public int EntityToken;

                [FieldOffset(0)]
                public Region FilterRegion;
            }
        }

        private readonly struct Region
        {
            public readonly LabelHandle Start;

            public readonly LabelHandle End;

            public Region(LabelHandle start, LabelHandle end)
            {
                Start = start;
                End = end;
            }
        }
    }
}

using System;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using ILAssembler.OpCodes;

namespace ILAssembler
{
    internal class CilAssembler : SignatureParser
    {
        private readonly CilAssemblyContext _context;

        private readonly ExceptionHandlerReader _ehReader;

        private bool _preambleFinished;

        private int? _maxStack;

        public CilAssembler(DynamicMethod method)
        {
            DynamicILInfo ilInfo = method.GetDynamicILInfo();
            _context = new CilAssemblyContext(ilInfo);
            _ehReader = new ExceptionHandlerReader(this, _context);
        }

        public static void CompileTo(ScriptBlockAst body, DynamicMethod method)
        {
            var compiler = new CilAssembler(method);
            body.Visit(compiler);

            var dest = new BlobBuilder();
            compiler._context.BranchBuilder.FixupCodeBuilder(dest);

            compiler._context.ILInfo.SetCode(
                dest.ToArray(),
                compiler._maxStack ?? 8);

            dest.Clear();
            compiler._context.BranchBuilder.SerializeExceptionTable(dest);
            compiler._context.ILInfo.SetExceptions(dest.ToArray());
            if (compiler._context.Locals is null)
            {
                compiler._context.ILInfo.SetLocalSignature(new byte[] { 7, 0 });
            }
        }

        public override void VisitNamedBlock(NamedBlockAst namedBlockAst)
        {
            foreach (StatementAst statement in namedBlockAst.Statements)
            {
                statement.Visit(this);
            }

            if (namedBlockAst.Statements.Count is not 0)
            {
                _ehReader.MaybeFinalizePendingRegions(namedBlockAst.Extent);
            }
        }

        public override void VisitCommandExpression(CommandExpressionAst commandExpressionAst)
        {
            throw ErrorUnexpectedNode(commandExpressionAst);
        }

        public override void VisitThrowStatement(ThrowStatementAst throwStatementAst)
        {
            _ehReader.MaybeFinalizePendingRegions(throwStatementAst.Extent);
            _context.Encoder.OpCode(ILOpCode.Throw);
        }

        public override void VisitTryStatement(TryStatementAst tryStatementAst)
        {
            Throw.ParseException(
                tryStatementAst.Extent,
                nameof(Strings.TryStatementNotSupported),
                Strings.TryStatementNotSupported);
        }

        public override void VisitBreakStatement(BreakStatementAst breakStatementAst)
        {
            _ehReader.MaybeFinalizePendingRegions(breakStatementAst.Extent);
            _context.Encoder.OpCode(ILOpCode.Break);
        }

        public override void VisitCommand(CommandAst commandAst)
        {
            InstructionArguments arguments = commandAst.GetInstructionArguments();
            if (!_preambleFinished)
            {
                if (TryReadPreamble(commandAst, in arguments))
                {
                    return;
                }

                _preambleFinished = true;
            }

            if (_ehReader.TryProcessExceptionHandler(in arguments))
            {
                return;
            }

            string name = arguments.CommandName;
            int nameOffset = 0;
            if (name[^1] == ':')
            {
                LabelHandle label = _context.GetOrAddLabel(name[0..^1]);
                _context.BranchBuilder.MarkLabel(label);
                if (arguments.Count is 0)
                {
                    return;
                }

                arguments = arguments.Shift();
                name = arguments.CommandName;
                nameOffset = 1;
            }

            if (!OpCodeStore.TryGetOpCodeInfo(name, out OpCodeInfo? info))
            {
                throw Error.Parse(
                    commandAst.CommandElements[nameOffset],
                    nameof(Strings.UnrecognizedOpCode),
                    Strings.UnrecognizedOpCode);
            }

            info!.Emit(_context, in arguments);
        }

        private bool TryReadPreamble(CommandAst commandAst, in InstructionArguments arguments)
        {
            if (arguments.CommandName.Equals(".maxstack", StringComparison.OrdinalIgnoreCase))
            {
                if (_maxStack is not null)
                {
                    throw Error.Parse(
                        commandAst,
                        nameof(Strings.MaxStackAlreadySpecified),
                        Strings.MaxStackAlreadySpecified);
                }

                arguments.AssertArgumentCount(1);
                _maxStack = arguments[0].ReadNumber<int>();
                return true;
            }

            if (arguments.CommandName.Equals(".locals", StringComparison.OrdinalIgnoreCase))
            {
                if (_context.Locals is not null)
                {
                    throw Error.Parse(
                        commandAst.CommandElements[0],
                        nameof(Strings.LocalsAlreadySpecified),
                        Strings.LocalsAlreadySpecified);
                }

                ReadLocals(arguments);
                return true;
            }

            return false;
        }

        private unsafe void ReadLocals(InstructionArguments arguments)
        {
            _context.ILInfo.DynamicMethod.InitLocals = false;
            if (arguments.Count == 2)
            {
                if (arguments[0] is not (
                    StringConstantExpressionAst
                    and { StringConstantType: StringConstantType.BareWord, Value: "init" }))
                {
                    throw Error.Parse(
                        arguments[0],
                        nameof(Strings.UnexpectedLocalsKeyword),
                        Strings.UnexpectedLocalsKeyword);
                }

                _context.ILInfo.DynamicMethod.InitLocals = true;
                arguments = arguments.Slice(1);
            }

            if (arguments.Count is 0)
            {
                throw Error.Parse(
                    arguments.StartPosition.ToScriptExtent(),
                    nameof(Strings.MissingLocalsBody),
                    Strings.MissingLocalsBody);
            }
            else if (arguments.Count > 1)
            {
                var extentToThrow = ExtentOps.ExtentOf(
                    arguments[1].Extent,
                    arguments[^1].Extent);

                throw Error.Parse(
                    extentToThrow,
                    nameof(Strings.UnexpectedLocalsArgument),
                    Strings.UnexpectedLocalsArgument);
            }

            if (arguments[0] is not ScriptBlockExpressionAst sbExpression)
            {
                throw Error.UnexpectedType(arguments[0], nameof(ScriptBlock));
            }

            TypedIdentifier[] locals = LocalSignatureParser.ParseLocals(sbExpression.ScriptBlock);
            var blobEncoder = new BlobEncoder(new BlobBuilder());
            LocalVariablesEncoder encoder = blobEncoder.LocalVariableSignature(locals.Length);
            var names = new string[locals.Length];
            for (int i = 0; i < locals.Length; i++)
            {
                ref TypedIdentifier current = ref locals[i];
                names[i] = current.Name;
                LocalVariableTypeEncoder typeEncoder = encoder.AddVariable();
                if (current.Type == typeof(TypedReference))
                {
                    typeEncoder.TypedReference();
                    continue;
                }

                typeEncoder
                    .Type(current.IsByRef, current.IsPinned)
                    .Type(current.Type, _context.ILInfo);
            }

            _context.Locals = names;
            byte* rawSignature = stackalloc byte[encoder.Builder.Count];
            byte* c = rawSignature;
            foreach (Blob blob in blobEncoder.Builder.GetBlobs())
            {
                foreach (byte b in blob.GetBytes())
                {
                    *c++ = b;
                }
            }

            _context.ILInfo.SetLocalSignature(rawSignature, blobEncoder.Builder.Count);
        }
    }
}

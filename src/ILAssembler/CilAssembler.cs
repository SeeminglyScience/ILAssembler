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

        private bool _preambleFinished;

        private int? _maxStack;

        public CilAssembler(DynamicMethod method)
        {
            DynamicILInfo ilInfo = method.GetDynamicILInfo();
            _context = new CilAssemblyContext(ilInfo);
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
        }

        public override void VisitCommandExpression(CommandExpressionAst commandExpressionAst)
        {
            throw ErrorUnexpectedNode(commandExpressionAst);
        }

        public override void VisitThrowStatement(ThrowStatementAst throwStatementAst)
        {
            _context.Encoder.OpCode(ILOpCode.Throw);
        }

        public override void VisitTryStatement(TryStatementAst tryStatementAst)
        {
            var tryStart = _context.BranchBuilder.DefineLabel();
            var tryEnd = _context.BranchBuilder.DefineLabel();
            _context.BranchBuilder.MarkLabel(tryStart);
            foreach (StatementAst statement in tryStatementAst.Body.Statements)
            {
                statement.Visit(this);
            }

            LabelHandle? finallyStart = null;
            _context.BranchBuilder.MarkLabel(tryEnd);
            foreach (CatchClauseAst clause in tryStatementAst.CatchClauses)
            {
                Type catchType;
                if (clause.IsCatchAll)
                {
                    catchType = typeof(Exception);
                }
                else if (clause.CatchTypes.Count > 1)
                {
                    throw Error.Parse(
                        clause.CatchTypes[1],
                        nameof(Strings.MultipleCatchTypesNotSupported),
                        Strings.MultipleCatchTypesNotSupported);
                }
                else
                {
                    catchType = TypeResolver.Resolve(clause.CatchTypes[0].TypeName);
                }

                EntityHandle catchTypeHandle = _context.ILInfo.GetHandleFor(catchType.TypeHandle);
                var catchStart = _context.BranchBuilder.DefineLabel();
                var catchEnd = _context.BranchBuilder.DefineLabel();
                _context.BranchBuilder.MarkLabel(catchStart);
                foreach (StatementAst statement in clause.Body.Statements)
                {
                    statement.Visit(this);
                }

                _context.BranchBuilder.MarkLabel(catchEnd);
                _context.BranchBuilder.AddExceptionRegion(
                    ExceptionRegionKind.Catch,
                    tryStart,
                    tryEnd,
                    catchStart,
                    catchEnd,
                    catchType: catchTypeHandle);

                finallyStart = catchEnd;
            }

            if (tryStatementAst.Finally is not null)
            {
                if (finallyStart is null)
                {
                    finallyStart = _context.BranchBuilder.DefineLabel();
                    _context.BranchBuilder.MarkLabel(finallyStart.Value);
                }

                var finallyEnd = _context.BranchBuilder.DefineLabel();
                foreach (StatementAst statement in tryStatementAst.Finally.Statements)
                {
                    statement.Visit(this);
                }

                _context.BranchBuilder.MarkLabel(finallyEnd);
                _context.BranchBuilder.AddExceptionRegion(
                    ExceptionRegionKind.Finally,
                    tryStart,
                    finallyStart.Value,
                    finallyStart.Value,
                    finallyEnd);
            }
        }

        public override void VisitBreakStatement(BreakStatementAst breakStatementAst)
        {
            _context.Encoder.OpCode(ILOpCode.Break);
        }

        public override void VisitCommand(CommandAst commandAst)
        {
            if (!_preambleFinished)
            {
                if (TryReadPreamble(commandAst))
                {
                    return;
                }

                _preambleFinished = true;
            }

            string name = ReadCommandName(commandAst);
            if (name[0] == ':')
            {
                var label = _context.GetOrAddLabel(name.Substring(1));
                _context.BranchBuilder.MarkLabel(label);
                return;
            }

            if (!OpCodeStore.TryGetOpCodeInfo(name, out OpCodeInfo? info))
            {
                throw Error.Parse(
                    commandAst.CommandElements[0],
                    nameof(Strings.UnrecognizedOpCode),
                    Strings.UnrecognizedOpCode);
            }

            info!.Emit(_context, commandAst);
        }

        private bool TryReadPreamble(CommandAst commandAst)
        {
            var name = ReadCommandName(commandAst);
            if (name.Equals(".maxstack", StringComparison.OrdinalIgnoreCase))
            {
                if (_maxStack is not null)
                {
                    throw Error.Parse(
                        commandAst,
                        nameof(Strings.MaxStackAlreadySpecified),
                        Strings.MaxStackAlreadySpecified);
                }

                commandAst.AssertArgumentCount(1);
                _maxStack = commandAst.CommandElements[1].ReadNumber<int>();
                return true;
            }

            if (name.Equals(".locals", StringComparison.OrdinalIgnoreCase))
            {
                if (_context.Locals is not null)
                {
                    throw Error.Parse(
                        commandAst.CommandElements[0],
                        nameof(Strings.LocalsAlreadySpecified),
                        Strings.LocalsAlreadySpecified);
                }

                ReadLocals(commandAst);
                return true;
            }

            return false;
        }

        private unsafe void ReadLocals(CommandAst commandAst)
        {
            _context.ILInfo.DynamicMethod.InitLocals = false;
            Ast? body = null;
            if (commandAst.CommandElements.Count == 3)
            {
                if (!(commandAst.CommandElements[1] is StringConstantExpressionAst stringConstant)
                    || stringConstant.StringConstantType != StringConstantType.BareWord
                    || !stringConstant.Value.Equals("init", StringComparison.Ordinal))
                {
                    throw Error.Parse(
                        commandAst.CommandElements[1],
                        nameof(Strings.UnexpectedLocalsKeyword),
                        Strings.UnexpectedLocalsKeyword);
                }

                _context.ILInfo.DynamicMethod.InitLocals = true;
                body = commandAst.CommandElements[2];
            }
            else if (commandAst.CommandElements.Count == 2)
            {
                body = commandAst.CommandElements[1];
            }
            else if (commandAst.CommandElements.Count == 1)
            {
                throw Error.Parse(
                    commandAst.Extent.EndScriptPosition.ToScriptExtent(),
                    nameof(Strings.MissingLocalsBody),
                    Strings.MissingLocalsBody);
            }
            else
            {
                var extentToThrow = ExtentOps.ExtentOf(
                    commandAst.CommandElements[3].Extent,
                    commandAst.CommandElements[^1].Extent);

                throw Error.Parse(
                    extentToThrow,
                    nameof(Strings.UnexpectedLocalsArgument),
                    Strings.UnexpectedLocalsArgument);
            }

            if (!(body is ScriptBlockExpressionAst sbExpression))
            {
                throw Error.UnexpectedType(body, nameof(ScriptBlock));
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

        private static string ReadCommandName(CommandAst commandAst)
        {
            string name = commandAst.GetCommandName();
            if (!string.IsNullOrEmpty(name))
            {
                return name;
            }

            throw Error.Parse(
                commandAst,
                nameof(Strings.CannotReadCommandName),
                Strings.CannotReadCommandName);
        }
    }
}

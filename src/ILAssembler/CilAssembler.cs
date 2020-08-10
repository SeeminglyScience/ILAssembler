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
            ControlFlowBuilder flow = compiler._context.Encoder.ControlFlowBuilder!;
            if (compiler._context.Branches.Count > 0)
            {
                flow.CopyCodeAndFixupBranches(
                    compiler._context.Encoder.CodeBuilder,
                    dest);
            }
            else
            {
                compiler._context.Encoder.CodeBuilder.WriteContentTo(dest);
            }

            compiler._context.ILInfo.SetCode(
                dest.ToArray(),
                compiler._maxStack ?? 8);

            dest.Clear();
            flow.SerializeExceptionTable(dest);
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
            var tryStart = _context.Encoder.DefineLabel();
            var tryEnd = _context.Encoder.DefineLabel();
            _context.Encoder.MarkLabel(tryStart);
            foreach (StatementAst statement in tryStatementAst.Body.Statements)
            {
                statement.Visit(this);
            }

            _context.Encoder.MarkLabel(tryEnd);
            foreach (CatchClauseAst clause in tryStatementAst.CatchClauses)
            {
                Type catchType;
                if (clause.IsCatchAll)
                {
                    catchType = typeof(Exception);
                }
                else if (clause.CatchTypes.Count > 1)
                {
                    throw clause.CatchTypes[1].GetParseError(
                        "MultipleCatchTypesNotSupported",
                        "Only one catch type can be specified.");
                }
                else
                {
                    catchType = TypeResolver.Resolve(clause.CatchTypes[0].TypeName);
                }

                EntityHandle catchTypeHandle = _context.ILInfo.GetHandleFor(catchType.TypeHandle);
                var catchStart = _context.Encoder.DefineLabel();
                var catchEnd = _context.Encoder.DefineLabel();
                _context.Encoder.MarkLabel(catchStart);
                foreach (StatementAst statement in clause.Body.Statements)
                {
                    statement.Visit(this);
                }

                _context.Encoder.MarkLabel(catchEnd);
                _context.Encoder.ControlFlowBuilder!.AddCatchRegion(
                    tryStart,
                    tryEnd,
                    catchStart,
                    catchEnd,
                    catchTypeHandle);
            }

            if (tryStatementAst.Finally is not null)
            {
                var finallyStart = _context.Encoder.DefineLabel();
                var finallyEnd = _context.Encoder.DefineLabel();
                _context.Encoder.MarkLabel(finallyStart);
                foreach (StatementAst statement in tryStatementAst.Finally.Statements)
                {
                    statement.Visit(this);
                }

                _context.Encoder.MarkLabel(finallyEnd);
                _context.Encoder.ControlFlowBuilder!.AddFinallyRegion(
                    tryStart,
                    tryEnd,
                    finallyStart,
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
                _context.Encoder.MarkLabel(label);
                return;
            }

            if (!OpCodeStore.TryGetOpCodeInfo(name, out OpCodeInfo? info))
            {
                throw commandAst.CommandElements[0].GetParseError(
                    "UnrecognizedOpCode",
                    "Unrecognized CIL instruction.");
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
                    throw commandAst.GetParseError(
                        "MaxStackAlreadySpecified",
                        "Max stack size must be declared only once.");
                }

                commandAst.AssertArgumentCount(1);
                _maxStack = commandAst.CommandElements[1].ReadNumber<int>();
                return true;
            }

            if (name.Equals(".locals", StringComparison.OrdinalIgnoreCase))
            {
                if (_context.Locals is not null)
                {
                    throw commandAst.CommandElements[0].GetParseError(
                        "LocalsAlreadySpecified",
                        "Locals must be declared only once.");
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
                    throw commandAst.CommandElements[1].GetParseError(
                        "UnexpectedLocalsKeyword",
                        "Expected either bareword \"init\" modifier or locals body.");
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
                throw commandAst.Extent.EndScriptPosition.ToScriptExtent()
                    .GetParseError(
                        "MissingLocalsBody",
                        "Missing locals declaration body.");
            }
            else
            {
                var extentToThrow = ExtentOps.ExtentOf(
                    commandAst.CommandElements[3].Extent,
                    commandAst.CommandElements[commandAst.CommandElements.Count - 1].Extent);

                throw extentToThrow.GetParseError(
                    "UnexpectedArgument",
                    "Unexpected argument in locals declaration.");
            }

            if (!(body is ScriptBlockExpressionAst sbExpression))
            {
                throw body.ErrorUnexpectedType(nameof(ScriptBlock));
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

            throw commandAst.GetParseError("CannotReadCommandName", "Unable to determine element name.");
        }
    }
}

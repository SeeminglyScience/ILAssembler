using System;
using System.Diagnostics.CodeAnalysis;
using System.Management.Automation.Language;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace ILAssembler.OpCodes
{
    internal sealed class CalliOpCodeInfo : GeneralOpCodeInfo
    {
        public CalliOpCodeInfo(string name, ILOpCode opCode) : base(name, opCode)
        {
        }

        public override void Emit(CilAssemblyContext context, in InstructionArguments arguments)
        {
            if (arguments.Count < 1)
            {
                ThrowIncompleteCalli(arguments.StartPosition.ToScriptExtent());
            }

            SignatureCallingConvention? callingConvention = null;
            MethodIdentifier? signature = null;

            for (int i = 0; i < arguments.Count; i++)
            {
                if (arguments[i] is StringConstantExpressionAst stringConstant)
                {
                    if (callingConvention is not null)
                    {
                        throw ILParseException.Create(
                            stringConstant.Extent,
                            nameof(SR.UnexpectedCallingConvention),
                            SR.UnexpectedCallingConvention);
                    }

                    if (stringConstant.Value is "vararg")
                    {
                        callingConvention = SignatureCallingConvention.VarArgs;
                        continue;
                    }

                    if (stringConstant.Value is "default")
                    {
                        callingConvention = SignatureCallingConvention.Default;
                        continue;
                    }

                    if (stringConstant.Value is "unmanaged")
                    {
                        i++;
                        if (i >= arguments.Count || arguments[i] is not StringConstantExpressionAst nextString)
                        {
                            ThrowIncompleteCalli(
                                stringConstant.Extent.EndScriptPosition.ToScriptExtent());
                            return;
                        }

                        if (nextString.Value is "cdecl")
                        {
                            callingConvention = SignatureCallingConvention.CDecl;
                            continue;
                        }

                        if (nextString.Value is "stdcall")
                        {
                            callingConvention = SignatureCallingConvention.StdCall;
                            continue;
                        }

                        if (nextString.Value is "thiscall")
                        {
                            callingConvention = SignatureCallingConvention.ThisCall;
                            continue;
                        }

                        if (nextString.Value is "fastcall")
                        {
                            callingConvention = SignatureCallingConvention.FastCall;
                            continue;
                        }

                        throw ILParseException.Create(
                            nextString.Extent,
                            nameof(SR.UnknownUnmanagedCallingConvention),
                            SR.UnknownUnmanagedCallingConvention);
                    }

                    throw ILParseException.Create(
                        stringConstant.Extent,
                        nameof(SR.UnknownCallingConvention),
                        SR.UnknownCallingConvention);
                }

                if (arguments[i] is ScriptBlockExpressionAst scriptBlockExpression)
                {
                    if (i != arguments.Count - 1)
                    {
                        throw ILParseException.Create(
                            ExtentOps.ExtentOf(
                                arguments[i + 1].Extent,
                                arguments[^1].Extent),
                            nameof(SR.InvalidCalliArgument),
                            SR.InvalidCalliArgument);
                    }

                    callingConvention ??= SignatureCallingConvention.Default;
                    bool isManaged = callingConvention.Value == SignatureCallingConvention.Default
                        || callingConvention.Value == SignatureCallingConvention.VarArgs;
                    var parser = new MethodSignatureParser(
                        rejectCtor: true,
                        requireResolvableDeclaringType: isManaged);

                    scriptBlockExpression.ScriptBlock.Visit(parser);
                    signature = (MethodIdentifier)parser.GetMemberIdentifier(scriptBlockExpression.Extent);
                }
            }

            if (signature is null)
            {
                ThrowIncompleteCalli(
                    arguments[^1].Extent.EndScriptPosition.ToScriptExtent());
            }

            var blobEncoder = new BlobEncoder(new BlobBuilder());

            var encoder = blobEncoder.MethodSignature(
                callingConvention!.Value,
                signature.GenericArgs.Length,
                !signature.IsStatic);

            encoder.Parameters(
                signature.Parameters.Length,
                rt =>
                {
                    if (signature.ReturnType.GetModifiedType() == typeof(void))
                    {
                        rt.Void();
                        return;
                    }

                    if (signature.ReturnType.GetModifiedType() == typeof(TypeReference))
                    {
                        rt.TypedReference();
                        return;
                    }

                    rt.Type(signature.ReturnType.IsByRef).Type(signature.ReturnType.Type, context.ILInfo);
                },
                parameters =>
                {
                    for (int i = 0; i < signature.Parameters.Length; i++)
                    {
                        var type = signature.Parameters[i];
                        if (type.GetModifiedType() == typeof(TypedReference))
                        {
                            parameters.AddParameter().TypedReference();
                            continue;
                        }

                        parameters.AddParameter().Type(type.IsByRef).Type(type.Type, context.ILInfo);
                    }
            });

            int token = context.ILInfo.GetTokenFor(blobEncoder.Builder.ToArray());
            context.Encoder.OpCode(ILOpCode.Calli);
            context.Encoder.Token(token);
        }

        [DoesNotReturn, MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowIncompleteCalli(IScriptExtent subject)
        {
            throw ILParseException.Create(
                subject,
                nameof(SR.IncompleteCalli),
                SR.IncompleteCalli);
        }
    }
}

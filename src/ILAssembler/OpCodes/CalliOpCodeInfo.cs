using System;
using System.Management.Automation.Language;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

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
                throw Error.IncompleteCalli(arguments.StartPosition.ToScriptExtent());
            }

            SignatureCallingConvention? callingConvention = null;
            MethodIdentifier? signature = null;

            for (int i = 0; i < arguments.Count; i++)
            {
                if (arguments[i] is StringConstantExpressionAst stringConstant)
                {
                    if (callingConvention is not null)
                    {
                        throw Error.Parse(
                            stringConstant,
                            nameof(Strings.UnexpectedCallingConvention),
                            Strings.UnexpectedCallingConvention);
                    }

                    if (stringConstant.Value.Equals("vararg", StringComparison.Ordinal))
                    {
                        callingConvention = SignatureCallingConvention.VarArgs;
                        continue;
                    }

                    if (stringConstant.Value.Equals("default", StringComparison.Ordinal))
                    {
                        callingConvention = SignatureCallingConvention.Default;
                        continue;
                    }

                    if (stringConstant.Value.Equals("unmanaged", StringComparison.Ordinal))
                    {
                        i++;
                        if (i >= arguments.Count || !(arguments[i] is StringConstantExpressionAst nextString))
                        {
                            throw Error.IncompleteCalli(
                                stringConstant.Extent.EndScriptPosition.ToScriptExtent());
                        }

                        if (nextString.Value.Equals("cdecl", StringComparison.Ordinal))
                        {
                            callingConvention = SignatureCallingConvention.CDecl;
                            continue;
                        }

                        if (nextString.Value.Equals("stdcall", StringComparison.Ordinal))
                        {
                            callingConvention = SignatureCallingConvention.StdCall;
                            continue;
                        }

                        if (nextString.Value.Equals("thiscall", StringComparison.Ordinal))
                        {
                            callingConvention = SignatureCallingConvention.ThisCall;
                            continue;
                        }

                        if (nextString.Value.Equals("fastcall", StringComparison.Ordinal))
                        {
                            callingConvention = SignatureCallingConvention.FastCall;
                            continue;
                        }

                        throw Error.Parse(
                            nextString,
                            nameof(Strings.UnknownUnmanagedCallingConvention),
                            Strings.UnknownUnmanagedCallingConvention);
                    }

                    throw Error.Parse(
                        stringConstant,
                        nameof(Strings.UnknownCallingConvention),
                        Strings.UnknownCallingConvention);
                }

                if (arguments[i] is ScriptBlockExpressionAst scriptBlockExpression)
                {
                    if (i != arguments.Count - 1)
                    {
                        throw Error.Parse(
                            ExtentOps.ExtentOf(
                                arguments[i + 1].Extent,
                                arguments[^1].Extent),
                            nameof(Strings.InvalidCalliArgument),
                            Strings.InvalidCalliArgument);
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
                throw Error.IncompleteCalli(
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

            var token = context.ILInfo.GetTokenFor(blobEncoder.Builder.ToArray());
            context.Encoder.OpCode(ILOpCode.Calli);
            context.Encoder.Token(token);
        }
    }
}

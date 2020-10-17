using System;
using System.Collections.Concurrent;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Reflection;
using System.Reflection.Emit;

namespace ILAssembler
{
    /// <summary>
    /// Provides a binary entry point for the assembler.
    /// </summary>
    /// <remarks>
    /// This class is currently non-public. If you have a use case in mind for
    /// it, please open an issue and I'll make it public.
    /// </remarks>
    internal static class CilAssemblage
    {
        private static readonly ConcurrentDictionary<(Type, ScriptBlockAst), DynamicMethod> s_methodCache = new();

        public static Delegate CreateDelegate(ScriptBlockAst signature, ScriptBlockAst body)
        {
            return CreateDelegate(signature, body, skipCache: false);
        }

        public static Delegate CreateDelegate(ScriptBlockAst signature, ScriptBlockAst body, bool skipCache)
        {
            var signatureParser = new MethodSignatureParser(
                rejectCtor: true,
                requireResolvableDeclaringType: false);

            signature.Visit(signatureParser);
            var identifier = (MethodIdentifier)signatureParser.GetMemberIdentifier(signature.Extent);

            Type delegateType = DelegateTypeFactory.GetDelegateType(identifier);
            return CreateDelegate(delegateType, body, skipCache);
        }

        public static TDelegate CreateDelegate<TDelegate>(ScriptBlockAst body)
            where TDelegate : Delegate
        {
            return CreateDelegate<TDelegate>(body, skipCache: false);
        }

        public static TDelegate CreateDelegate<TDelegate>(ScriptBlockAst body, bool skipCache)
            where TDelegate : Delegate
        {
            return (TDelegate)CreateDelegate(typeof(TDelegate), body, skipCache);
        }

        public static Delegate CreateDelegate(Type delegateType, ScriptBlockAst body)
        {
            return CreateDelegate(delegateType, body, skipCache: false);
        }

        public static Delegate CreateDelegate(Type delegateType, ScriptBlockAst body, bool skipCache)
        {
            DynamicMethod method = CreateCilMethod(delegateType, body, skipCache);
            return method.CreateDelegate(delegateType);
        }

        private static DynamicMethod CreateCilMethod(Type delegateType, ScriptBlockAst body, bool skipCache = false)
        {
            if (skipCache)
            {
                return CreateCilMethod((delegateType, body));
            }

            return s_methodCache.GetOrAdd(
                (delegateType, body),
                args => CreateCilMethod(args));
        }

        private static DynamicMethod CreateCilMethod((Type delegateType, ScriptBlockAst body) args)
        {
            (Type delegateType, ScriptBlockAst body) = args;
            DynamicMethod method = CreateDynamicMethod(delegateType);
            CilAssembler.CompileTo(body, method);
            return method;
        }

        private static DynamicMethod CreateDynamicMethod(ScriptBlockAst signature, out Type delegateType)
        {
            var signatureParser = new MethodSignatureParser(
                rejectCtor: true,
                requireResolvableDeclaringType: false);

            signature.Visit(signatureParser);
            var identifier = (MethodIdentifier)signatureParser.GetMemberIdentifier(signature.Extent);

            delegateType = DelegateTypeFactory.GetDelegateType(identifier);

            return new DynamicMethod(
                identifier.Name,
                identifier.ReturnType.GetModifiedType(),
                identifier.Parameters.ToModifiedTypeArray(),
                typeof(CilAssemblage).Module,
                skipVisibility: true);
        }

        private static DynamicMethod CreateDynamicMethod(Type delegateType)
        {
            if (!delegateType.IsSubclassOf(typeof(Delegate)))
            {
                throw new ArgumentException(SR.InvalidDelegateType, nameof(delegateType))
                    .WithErrorInfo(
                        nameof(SR.InvalidDelegateType),
                        ErrorCategory.InvalidArgument,
                        delegateType);
            }

            MethodInfo? invokeMethod = delegateType.GetMethod(nameof(Action.Invoke));
            if (invokeMethod is null)
            {
                throw new ArgumentException(SR.DelegateTypeMissingInvoke, nameof(delegateType))
                    .WithErrorInfo(
                        nameof(SR.DelegateTypeMissingInvoke),
                        ErrorCategory.InvalidArgument,
                        delegateType);
            }

            ParameterInfo[] parameters = invokeMethod.GetParameters();
            var parameterTypes = new Type[parameters.Length];
            for (int i = parameterTypes.Length - 1; i >= 0; i--)
            {
                parameterTypes[i] = parameters[i].ParameterType;
            }

            return new DynamicMethod(
                invokeMethod.Name,
                invokeMethod.ReturnType,
                parameterTypes,
                typeof(CilAssemblage).Module,
                skipVisibility: true);
        }
    }
}

using System;
using System.Management.Automation.Language;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace ILAssembler
{
    internal class MethodIdentifier : MemberIdentifier
    {
        public MethodIdentifier(
            TypedIdentifier? declaringType,
            string? name,
            bool isStatic,
            TypedIdentifier returnType,
            TypedIdentifier[]? genericArgs,
            TypedIdentifier[]? parameters)
            : base(declaringType, name ?? string.Empty, isStatic)
        {
            ReturnType = returnType;
            GenericArgs = genericArgs ?? Array.Empty<TypedIdentifier>();
            Parameters = parameters ?? Array.Empty<TypedIdentifier>();
        }

        public TypedIdentifier ReturnType { get; }

        public TypedIdentifier[] GenericArgs { get; }

        public TypedIdentifier[] Parameters { get; }

        public bool TryGetDelegateInvokeMethod(out MethodInfo? invokeMethod)
        {
            invokeMethod = null;
            Type? declaringType = DeclaringType?.Type;
            if (declaringType is null || !declaringType.IsSubclassOf(typeof(Delegate)))
            {
                return false;
            }

            if (IsStatic || Name.Equals(nameof(Action.Invoke), StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            try
            {
                invokeMethod = declaringType.GetMethod(
                    Name,
                    BindingFlags.Instance | BindingFlags.Public,
                    binder: null,
                    Parameters.ToModifiedTypeArray(),
                    modifiers: null);
            }
            catch
            {
                // Just ignore and treat as failure.
            }

            return invokeMethod is not null;
        }

        public override int GetToken(CilAssemblyContext context, IScriptExtent subject)
        {
            var method = GetMethod(subject);
            if (method.DeclaringType is null)
            {
                throw Error.Parse(
                    subject,
                    nameof(Strings.MissingDeclaringType),
                    Strings.MissingDeclaringType);
            }

            if (method.DeclaringType.IsGenericType)
            {
                return context.ILInfo.GetTokenFor(method.MethodHandle, method.DeclaringType.TypeHandle);
            }

            return context.ILInfo.GetTokenFor(method.MethodHandle);
        }

        private MethodInfo GetMethod(IScriptExtent subject)
        {
            if (DeclaringType?.Type is null)
            {
                throw Error.Parse(
                    subject,
                    nameof(Strings.MissingDeclaringType),
                    Strings.MissingDeclaringType);
            }

            if (GenericArgs.Length == 0)
            {
                var parameterTypes = Parameters.ToModifiedTypeArray();

                MethodInfo? resolvedMethod = DeclaringType.Type.GetMethod(
                    Name,
                    GetFlags(),
                    binder: null,
                    parameterTypes,
                    modifiers: null);

                if (resolvedMethod is null)
                {
                    throw ErrorMemberNotFound(subject);
                }

                return resolvedMethod;
            }

            var candidates = DeclaringType.Type.GetMember(
                Name,
                MemberTypes.Method,
                GetFlags());

            foreach (MethodInfo method in candidates)
            {
                if (!method.IsGenericMethod)
                {
                    continue;
                }

                int genericArgumentCount = method.GetGenericArguments().Length;
                if (genericArgumentCount != GenericArgs.Length)
                {
                    continue;
                }

                if (method.GetParameters().Length != Parameters.Length)
                {
                    continue;
                }

                MethodInfo concreteMethod;
                try
                {
                    concreteMethod = method.MakeGenericMethod(GenericArgs.ToModifiedTypeArray());
                }
                catch (Exception e)
                {
                    throw Error.Parse(
                        subject,
                        "InvalidGenericArgs",
                        ExceptionDispatchInfo.Capture(e));
                }

                ParameterInfo[] candidateParameters = concreteMethod.GetParameters();
                bool isMatch = true;
                for (int i = 0; i < candidateParameters.Length; i++)
                {
                    if (candidateParameters[i].ParameterType != Parameters[i].GetModifiedType())
                    {
                        isMatch = false;
                        break;
                    }
                }

                if (!isMatch)
                {
                    break;
                }

                return concreteMethod;
            }

            throw ErrorMemberNotFound(subject);
        }
    }
}

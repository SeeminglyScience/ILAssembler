using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace ILAssembler
{
    internal sealed class DelegateTypeFactory
    {
        private static readonly Lazy<ModuleBuilder> s_module = new(
            () =>
            {
                AssemblyBuilder assembly = AssemblyBuilder.DefineDynamicAssembly(
                    new AssemblyName("PSILGen_Dynamic_Assembly"),
                    AssemblyBuilderAccess.Run);

                return assembly.DefineDynamicModule("PSILGen_Dynamic_Module");
            });

        private static int s_anonymousDelegateId;

        private static readonly ConcurrentDictionary<SignatureKey, Type> s_anonymousTypes = new();

        public static Type GetDelegateType(MethodIdentifier method)
        {
            var signatureKey = new SignatureKey(method.ReturnType, method.Parameters);

            if (CanBeGenericDelegate(in signatureKey))
            {
                Type returnType = method.ReturnType.Type;
                bool isVoidReturn = returnType == typeof(void);
                Type genericDelegate = GetGenericDelegateType(method.Parameters.Length, isVoidReturn);

                if (isVoidReturn)
                {
                    return genericDelegate.IsGenericTypeDefinition
                        ? genericDelegate.MakeGenericType(method.Parameters.ToModifiedTypeArray())
                        : genericDelegate;
                }

                var funcArgs = new Type[method.Parameters.Length + 1];
                for (int i = 0; i < method.Parameters.Length; i++)
                {
                    funcArgs[i] = method.Parameters[i].GetModifiedType();
                }

                funcArgs[^1] = returnType;
                return genericDelegate.MakeGenericType(funcArgs);
            }

            return s_anonymousTypes.GetOrAdd(
                signatureKey,
                key => CreateDelegateBySignatureKey(in key));
        }

        public static Type CreateDelegateBySignatureKey(in SignatureKey key)
        {
            string typeName = string.Format(
                CultureInfo.InvariantCulture,
                "PSILGen.GeneratedDelegates.AnonymousDelegate_{0}",
                Interlocked.Increment(ref s_anonymousDelegateId));

            return CreateDelegateBySignatureKey(typeName, in key);
        }

        public static Type CreateDelegateBySignatureKey(string typeName, in SignatureKey key)
        {
            TypeBuilder type = s_module.Value.DefineType(
                typeName,
                TypeAttributes.NotPublic | TypeAttributes.Sealed,
                typeof(MulticastDelegate));

            MethodBuilder method = type.DefineMethod(
                nameof(Action.Invoke),
                MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.NewSlot,
                key.ReturnType.GetModifiedType(),
                key.Parameters.ToModifiedTypeArray());

            for (int i = 0; i < key.Parameters.Length; i++)
            {
                method.DefineParameter(
                    i + 1,
                    ParameterAttributes.None,
                    key.Parameters[i].Name);
            }

            method.SetImplementationFlags(MethodImplAttributes.Runtime);

            ConstructorBuilder constructor = type.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.RTSpecialName | MethodAttributes.HideBySig,
                CallingConventions.Standard,
                new[] { typeof(object), typeof(IntPtr) });

            constructor.SetImplementationFlags(MethodImplAttributes.Runtime);
            return type.CreateType()!;
        }

        internal static bool IsSignatureMatch(in SignatureKey key, MethodInfo method)
        {
            if (key.ReturnType.GetModifiedType() != method.ReturnType)
            {
                return false;
            }

            var parameters = method.GetParameters();
            if (parameters.Length != key.Parameters.Length)
            {
                return false;
            }

            if (parameters.Length == 0)
            {
                return true;
            }

            for (int i = parameters.Length - 1; i >= 0; i--)
            {
                if (parameters[i].ParameterType != key.Parameters[i].GetModifiedType())
                {
                    return false;
                }
            }

            return true;
        }

        private static bool CanBeGenericDelegate(in SignatureKey key)
        {
            if (!CanBeGenericParameter(key.ReturnType))
            {
                return false;
            }

            if (key.Parameters.Length > 16)
            {
                return false;
            }

            foreach (TypedIdentifier type in key.Parameters)
            {
                if (!CanBeGenericParameter(type))
                {
                    return false;
                }
            }

            return true;
        }

        private static Type GetGenericDelegateType(int parameterCount, bool hasVoidReturn)
        {
            if (hasVoidReturn)
            {
                return parameterCount switch
                {
                    0 => typeof(Action),
                    1 => typeof(Action<>),
                    2 => typeof(Action<,>),
                    3 => typeof(Action<,,>),
                    4 => typeof(Action<,,,>),
                    5 => typeof(Action<,,,,>),
                    6 => typeof(Action<,,,,,>),
                    7 => typeof(Action<,,,,,,>),
                    8 => typeof(Action<,,,,,,,>),
                    9 => typeof(Action<,,,,,,,,>),
                    10 => typeof(Action<,,,,,,,,,>),
                    11 => typeof(Action<,,,,,,,,,,>),
                    12 => typeof(Action<,,,,,,,,,,,>),
                    13 => typeof(Action<,,,,,,,,,,,,>),
                    14 => typeof(Action<,,,,,,,,,,,,,>),
                    15 => typeof(Action<,,,,,,,,,,,,,,>),
                    16 => typeof(Action<,,,,,,,,,,,,,,,>),
                    _ => throw new ArgumentOutOfRangeException(nameof(parameterCount)),
                };
            }

            return parameterCount switch
            {
                0 => typeof(Func<>),
                1 => typeof(Func<,>),
                2 => typeof(Func<,,>),
                3 => typeof(Func<,,,>),
                4 => typeof(Func<,,,,>),
                5 => typeof(Func<,,,,,>),
                6 => typeof(Func<,,,,,,>),
                7 => typeof(Func<,,,,,,,>),
                8 => typeof(Func<,,,,,,,,>),
                9 => typeof(Func<,,,,,,,,,>),
                10 => typeof(Func<,,,,,,,,,,>),
                11 => typeof(Func<,,,,,,,,,,,>),
                12 => typeof(Func<,,,,,,,,,,,,>),
                13 => typeof(Func<,,,,,,,,,,,,,>),
                14 => typeof(Func<,,,,,,,,,,,,,,>),
                15 => typeof(Func<,,,,,,,,,,,,,,,>),
                16 => typeof(Func<,,,,,,,,,,,,,,,,>),
                _ => throw new ArgumentOutOfRangeException(nameof(parameterCount)),
            };
        }

        private static bool CanBeGenericParameter(TypedIdentifier typeInfo)
        {
            if (typeInfo.IsPinned)
            {
                return false;
            }

            Type type = typeInfo.GetModifiedType();
            if (type.IsByRef)
            {
                return false;
            }

            if (type.IsPointer)
            {
                return false;
            }

            if (type == typeof(TypedReference))
            {
                return false;
            }

            if (type == typeof(ArgIterator))
            {
                return false;
            }

            foreach (object attribute in type.GetCustomAttributes(false))
            {
                // FullName is only null when the type is a generic parameter.
                bool isByRefLike = attribute is Attribute attrib
                    && attrib.TypeId is Type attribType
                    && attribType.FullName!.Equals(
                        "System.Runtime.CompilerServices.IsByRefLikeAttribute",
                        StringComparison.Ordinal);

                if (isByRefLike)
                {
                    return false;
                }
            }

            return true;
        }
    }
}

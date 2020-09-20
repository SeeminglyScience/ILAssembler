using System;
using System.Management.Automation.Language;
using System.Reflection;

namespace ILAssembler
{
    internal class ConstructorIdentifier : MemberIdentifier
    {
        public ConstructorIdentifier(
            TypedIdentifier? declaringType,
            bool isStatic,
            TypedIdentifier returnType,
            TypedIdentifier[]? parameters)
            : base(declaringType, isStatic ? ".cctor" : ".ctor", isStatic)
        {
            ReturnType = returnType;
            Parameters = parameters ?? Array.Empty<TypedIdentifier>();
        }

        public TypedIdentifier ReturnType { get; }

        public TypedIdentifier[] Parameters { get; }

        public override int GetToken(CilAssemblyContext context, IScriptExtent subject)
        {
            var parameterTypes = new Type[Parameters.Length];
            for (int i = 0; i < parameterTypes.Length; i++)
            {
                parameterTypes[i] = Parameters[i].GetModifiedType()!;
            }

            if (DeclaringType?.Type is null)
            {
                throw ILParseException.Create(
                    subject,
                    nameof(SR.MissingDeclaringType),
                    SR.MissingDeclaringType);
            }

            ConstructorInfo? resolvedCtor = DeclaringType.Type.GetConstructor(
                GetFlags(),
                binder: null,
                parameterTypes,
                modifiers: null);

            if (resolvedCtor is null)
            {
                throw ErrorMemberNotFound(subject);
            }

            if (resolvedCtor.DeclaringType!.IsGenericType)
            {
                return context.ILInfo.GetTokenFor(
                    resolvedCtor.MethodHandle,
                    resolvedCtor.DeclaringType.TypeHandle);
            }

            return context.ILInfo.GetTokenFor(resolvedCtor.MethodHandle);
        }
    }
}

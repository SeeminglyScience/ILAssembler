using System;
using System.Diagnostics.CodeAnalysis;
using System.Management.Automation.Language;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace ILAssembler
{
    internal static class TypeResolver
    {
        public static Type Resolve(ITypeName typeName)
        {
            if (!(typeName.IsArray || typeName.IsGeneric) && typeName.FullName[^1] == '+')
            {
                Type type = Resolve(
                    new TypeName(
                        typeName.Extent,
                        typeName.FullName[0..^1]));

                return type.MakePointerType();
            }

            if (typeName is not GenericTypeName genericTypeName)
            {
                return typeName.GetReflectionType() ?? ThrowTypeNotFound(typeName.Extent);
            }

            if (genericTypeName.TypeName.FullName.Equals(SpecialTypes.ByRef, StringComparison.Ordinal)
                || genericTypeName.TypeName.FullName.Equals(SpecialTypes.Ref, StringComparison.Ordinal))
            {
                AssertSingleGenericArgument(genericTypeName);
                var realType = Resolve(genericTypeName.GenericArguments[0]);
                return realType.MakeByRefType();
            }

            if (genericTypeName.TypeName.FullName.Equals(SpecialTypes.Pointer, StringComparison.Ordinal))
            {
                AssertSingleGenericArgument(genericTypeName);
                var realType = Resolve(genericTypeName.GenericArguments[0]);
                return realType.MakePointerType();
            }

            Type genericDefinition = GetGenericTypeDefinition(
                genericTypeName.TypeName,
                genericTypeName.GenericArguments.Count);

            var resolvedArguments = new Type[genericTypeName.GenericArguments.Count];
            for (int i = 0; i < resolvedArguments.Length; i++)
            {
                resolvedArguments[i] = Resolve(genericTypeName.GenericArguments[i]);
            }

            try
            {
                return genericDefinition.MakeGenericType(resolvedArguments);
            }
            catch (Exception e)
            {
                throw ILParseException.Create(
                    typeName.Extent,
                    "InvalidGenericArguments",
                    ExceptionDispatchInfo.Capture(e));
            }
        }

        private static Type GetGenericTypeDefinition(ITypeName typeName, int arity)
        {
            Type type = typeName.GetReflectionType();
            if ((type is null || !type.ContainsGenericParameters) && typeName.FullName.IndexOf('`') == -1)
            {
                type = new TypeName(
                    typeName.Extent,
                    $"{typeName.FullName}`{arity}")
                    .GetReflectionType();
            }

            if (type is null)
            {
                ThrowTypeNotFound(typeName.Extent);
            }

            return type;
        }

        [DoesNotReturn, MethodImpl(MethodImplOptions.NoInlining)]
        private static Type ThrowTypeNotFound(IScriptExtent extent)
        {
            throw ILParseException.Create(
                extent,
                nameof(SR.TypeNotFound),
                SR.Format(SR.TypeNotFound, extent.Text));
        }

        private static void AssertSingleGenericArgument(GenericTypeName typeName)
        {
            if (typeName.GenericArguments.Count == 1)
            {
                return;
            }

            IScriptExtent extentToThrow = ExtentOps.ExtentOf(
                typeName.GenericArguments[1].Extent,
                typeName.GenericArguments[^1].Extent);

            Throw.ParseException(
                extentToThrow,
                nameof(SR.SingleGenericArgumentExpected),
                SR.Format(SR.SingleGenericArgumentExpected, typeName.TypeName));
        }
    }
}

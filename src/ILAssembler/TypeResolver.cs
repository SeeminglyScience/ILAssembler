using System;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Runtime.ExceptionServices;

namespace ILAssembler
{
    internal static class TypeResolver
    {
        public static Type Resolve(ITypeName typeName)
        {
            if (!(typeName is GenericTypeName genericTypeName))
            {
                return typeName.GetReflectionType() ?? throw ErrorTypeNotFound(typeName.Extent);
            }

            if (genericTypeName.TypeName.FullName.Equals(SpecialTypes.ByRef, StringComparison.Ordinal))
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

            var genericDefinition = GetGenericTypeDefinition(
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
                throw Error.Parse(
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
                throw ErrorTypeNotFound(typeName.Extent);
            }

            return type;
        }

        private static ParseException ErrorTypeNotFound(IScriptExtent extent)
        {
            return Error.Parse(
                extent,
                nameof(Strings.TypeNotFound),
                Strings.TypeNotFound,
                extent.Text);
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

            throw Error.Parse(
                extentToThrow,
                nameof(Strings.SingleGenericArgumentExpected),
                Strings.SingleGenericArgumentExpected,
                typeName.TypeName);
        }
    }
}

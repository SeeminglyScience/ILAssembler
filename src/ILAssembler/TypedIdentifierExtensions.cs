using System;

namespace ILAssembler
{
    internal static class TypedIdentifierExtensions
    {
        public static Type[] ToModifiedTypeArray(this TypedIdentifier[] types)
        {
            var result = new Type[types.Length];
            for (int i = types.Length - 1; i >= 0; i--)
            {
                result[i] = types[i].GetModifiedType();
            }

            return result;
        }
    }
}

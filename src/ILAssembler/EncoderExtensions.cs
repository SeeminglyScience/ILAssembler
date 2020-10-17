using System;
using System.Collections.Immutable;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace ILAssembler
{
    internal static class EncoderExtensions
    {
        public static EntityHandle GetHandleFor(this DynamicILInfo ilInfo, RuntimeTypeHandle typeHandle)
        {
            int token = ilInfo.GetTokenFor(typeHandle);
            EntityHandle handle = MetadataTokens.EntityHandle(token);
            return handle;
        }

        public static void Type(this SignatureTypeEncoder encoder, Type type, DynamicILInfo ilInfo)
        {
            if (type.IsArray)
            {
                int rank = type.GetArrayRank();
                if (rank == 1)
                {
                    Type(encoder.SZArray(), type.GetElementType()!, ilInfo);
                    return;
                }

                encoder.Array(
                    elementType => elementType.Type(type.GetElementType()!, ilInfo),
                    shape => shape.Shape(rank, ImmutableArray<int>.Empty, ImmutableArray<int>.Empty));
                return;
            }

            if (type.IsGenericParameter)
            {
                if (type.DeclaringMethod is not null)
                {
                    encoder.GenericMethodTypeParameter(type.GenericParameterPosition);
                    return;
                }

                encoder.GenericTypeParameter(type.GenericParameterPosition);
            }

            if (type.IsGenericType)
            {
                // The supported way of declaring a generic instantiation isn't
                // working correctly in a locals sig. Unsure if it's something
                // we're doing wrong or if it's a problem with the DynamicILInfo
                // API. As a workaround, we're depending on an implementation
                // detail used by the runtime to embed a type handle directly
                // as a pointer.
                //
                // The proper implementation is commented below this.
                //
                // https://github.com/SeeminglyScience/ILAssembler/issues/42
                const byte ELEMENT_TYPE_INTERNAL = 0x21;
                encoder.Builder.WriteByte(ELEMENT_TYPE_INTERNAL);
                IntPtr rawHandle = type.TypeHandle.Value;
                unsafe
                {
                    encoder.Builder.WriteBytes(
                        (byte*)&rawHandle,
                        sizeof(IntPtr));
                }

                // Type genericDefinition = type.GetGenericTypeDefinition();
                // Type[] genericArguments = type.GetGenericArguments();
                // GenericTypeArgumentsEncoder argumentsEncoder = encoder.GenericInstantiation(
                //     ilInfo.GetHandleFor(genericDefinition.TypeHandle),
                //     genericArguments.Length,
                //     genericDefinition.IsValueType);

                // foreach (Type genericArgument in genericArguments)
                // {
                //     Type(argumentsEncoder.AddArgument(), genericArgument, ilInfo);
                // }

                return;
            }

            switch (System.Type.GetTypeCode(type))
            {
                case TypeCode.Boolean: encoder.Boolean(); return;
                case TypeCode.Char: encoder.Char(); return;
                case TypeCode.Double: encoder.Double(); return;
                case TypeCode.SByte: encoder.SByte(); return;
                case TypeCode.Int16: encoder.Int16(); return;
                case TypeCode.Int32: encoder.Int32(); return;
                case TypeCode.Int64: encoder.Int64(); return;
                case TypeCode.Byte: encoder.Byte(); return;
                case TypeCode.UInt16: encoder.UInt16(); return;
                case TypeCode.UInt32: encoder.UInt32(); return;
                case TypeCode.UInt64: encoder.UInt64(); return;
                case TypeCode.String: encoder.String(); return;
            }

            if (type == typeof(IntPtr))
            {
                encoder.IntPtr();
                return;
            }

            if (type == typeof(UIntPtr))
            {
                encoder.UIntPtr();
                return;
            }

            if (type == typeof(object))
            {
                encoder.Object();
                return;
            }

            if (type == typeof(void*))
            {
                encoder.VoidPointer();
                return;
            }

            if (type.IsPointer)
            {
                Type(encoder.Pointer(), type.GetElementType()!, ilInfo);
                return;
            }

            if (type.IsByRef)
            {
                // byref not supported in LocalSignatureEncoder, but byref locals work fine
                // so manually encoding.
                const int ELEMENT_TYPE_BYREF = 0x10;
                encoder.Builder.WriteByte(ELEMENT_TYPE_BYREF);
                Type(encoder, type.GetElementType()!, ilInfo);
                return;
            }

            encoder.Type(
                ilInfo.GetHandleFor(type.TypeHandle),
                type.IsValueType);
        }
    }
}

using System;

namespace ILAssembler
{
    internal class TypedIdentifier : IEquatable<TypedIdentifier>
    {
        public readonly Type Type;

        public readonly string Name;

        public readonly bool IsPinned;

        public readonly bool IsByRef;

        public TypedIdentifier(Type type, string? name, bool isPinned, bool isByRef)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Name = name ?? string.Empty;
            IsPinned = isPinned;
            IsByRef = isByRef;
        }

        public static TypedIdentifier Void => new TypedIdentifier(
            typeof(void),
            string.Empty,
            isPinned: false,
            isByRef: false);

        public static implicit operator TypedIdentifier((Type type, string? name, bool isPinned, bool isByRef) value)
        {
            return new TypedIdentifier(value.type, value.name, value.isPinned, value.isByRef);
        }

        public static implicit operator TypedIdentifier(Type type)
        {
            if (type.IsByRef)
            {
                return new TypedIdentifier(type.GetElementType()!, string.Empty, isPinned: false, isByRef: true);
            }

            return new TypedIdentifier(type, string.Empty, isPinned: false, isByRef: false);
        }

        public static bool operator ==(TypedIdentifier left, TypedIdentifier right)
        {
            if (left is not null)
            {
                return left.Equals(right);
            }

            return right is null;
        }

        public static bool operator !=(TypedIdentifier left, TypedIdentifier right)
        {
            if (left is not null)
            {
                return !left.Equals(right);
            }

            return right is not null;
        }

        public void Deconstruct(out Type type, out string name, out bool isPinned, out bool isByRef)
        {
            type = Type;
            name = Name;
            isPinned = IsPinned;
            isByRef = IsByRef;
        }

        public Type GetModifiedType()
        {
            return IsByRef ? Type.MakeByRefType() : Type;
        }

        public bool Equals(TypedIdentifier? other)
        {
            if (other is null)
            {
                return false;
            }

            if (Name is null)
            {
                if (other.Name is not null)
                {
                    return false;
                }
            }
            else if (!Name.Equals(other.Name, StringComparison.Ordinal))
            {
                return false;
            }

            return Type == other.Type
                && IsByRef == other.IsByRef
                && IsPinned == other.IsPinned;
        }

        public override bool Equals(object? obj)
            => obj is TypedIdentifier typedIdentifier && Equals(typedIdentifier);

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = (hash * 23) + (Name?.GetHashCode() ?? 0);
                hash = (hash * 23) + (Type?.GetHashCode() ?? 0);
                hash = (hash * 23) + IsByRef.GetHashCode();
                hash = (hash * 23) + IsPinned.GetHashCode();
                return hash;
            }
        }
    }
}

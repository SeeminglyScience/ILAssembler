using System;

namespace ILAssembler
{
    internal readonly struct SignatureKey : IEquatable<SignatureKey>
    {
        public SignatureKey(TypedIdentifier returnType, TypedIdentifier[] parameters)
        {
            ReturnType = returnType;
            Parameters = parameters;
        }

        public readonly TypedIdentifier ReturnType;

        public readonly TypedIdentifier[] Parameters;

        public static bool operator ==(SignatureKey left, SignatureKey right) => left.Equals(right);

        public static bool operator !=(SignatureKey left, SignatureKey right) => !left.Equals(right);

        public bool Equals(SignatureKey other)
        {
            if (!ReturnType.Equals(other.ReturnType))
            {
                return false;
            }

            if (Parameters.Length != other.Parameters.Length)
            {
                return false;
            }

            if (Parameters.Length == 0)
            {
                return true;
            }

            for (int i = 0; i < Parameters.Length; i++)
            {
                if (!Parameters[i].Equals(other.Parameters[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = (hash * 23) + (ReturnType?.GetHashCode() ?? 0);
                if (Parameters == null || Parameters.Length == 0)
                {
                    return hash;
                }

                foreach (TypedIdentifier type in Parameters)
                {
                    hash = (hash * 23) + (type?.GetHashCode() ?? 0);
                }

                return hash;
            }
        }

        public override bool Equals(object? obj) => obj is SignatureKey key && Equals(key);
    }
}

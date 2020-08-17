namespace ILAssembler
{
    internal static class HashCode
    {
#if CORE
        public static int Combine<T1>(T1 value1) => System.HashCode.Combine(value1);
        public static int Combine<T1, T2>(T1 value1, T2 value2) => System.HashCode.Combine(value1, value2);
        public static int Combine<T1, T2, T3>(T1 value1, T2 value2, T3 value3) => System.HashCode.Combine(value1, value2, value3);
        public static int Combine<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3, T4 value4) => System.HashCode.Combine(value1, value2, value3, value4);
#else
        public static int Combine<T1>(T1 value1)
        {
            unchecked
            {
                int hash = 17;
                hash = (hash * 23) + (value1?.GetHashCode() ?? 0);
                return hash;
            }
        }

        public static int Combine<T1, T2>(T1 value1, T2 value2)
        {
            unchecked
            {
                int hash = 17;
                hash = (hash * 23) + (value1?.GetHashCode() ?? 0);
                hash = (hash * 23) + (value2?.GetHashCode() ?? 0);
                return hash;
            }
        }

        public static int Combine<T1, T2, T3>(T1 value1, T2 value2, T3 value3)
        {
            unchecked
            {
                int hash = 17;
                hash = (hash * 23) + (value1?.GetHashCode() ?? 0);
                hash = (hash * 23) + (value2?.GetHashCode() ?? 0);
                hash = (hash * 23) + (value3?.GetHashCode() ?? 0);
                return hash;
            }
        }

        public static int Combine<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3, T4 value4)
        {
            unchecked
            {
                int hash = 17;
                hash = (hash * 23) + (value1?.GetHashCode() ?? 0);
                hash = (hash * 23) + (value2?.GetHashCode() ?? 0);
                hash = (hash * 23) + (value3?.GetHashCode() ?? 0);
                hash = (hash * 23) + (value4?.GetHashCode() ?? 0);
                return hash;
            }
        }
#endif
    }
}

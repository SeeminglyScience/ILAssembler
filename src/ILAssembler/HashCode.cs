namespace ILAssembler
{
    internal static class HashCode
    {
#if CORE
        public static int Combine<T1>(T1 value1) => System.HashCode.Combine(value1);
        public static int Combine<T1, T2>(T1 value1, T2 value2) => System.HashCode.Combine(value1, value2);
        public static int Combine<T1, T2, T3>(T1 value1, T2 value2, T3 value3) => System.HashCode.Combine(value1, value2, value3);
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
#endif
    }
}

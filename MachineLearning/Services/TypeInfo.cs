using System;
using System.Reflection;
using Analysis.Attributes;

namespace Analysis.Services
{
    public static class TypeInfo
    {
        public static Type ConditionAttribute { get; } = typeof(ConditionAttribute);
        public static Type OutcomeAttribute { get; } = typeof(OutcomeAttribute);
    }

    public static class TypeInfo<T>
    {
        public static Type Type { get; } = typeof(T);
        public static PropertyInfo[] Properties { get; } = Type.GetProperties();
    }
}
using System;

namespace Data.Services
{
    public static class Types
    {
        public static readonly Type String = typeof(string);
        public static readonly Type ValueType = typeof(ValueType);
        public static readonly Type Nullable = typeof(Nullable<>);
    }
}
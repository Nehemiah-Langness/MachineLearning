using System;
using System.Collections.Generic;
using System.Reflection;
using Analysis.Attributes;

namespace Analysis.Services
{
    public static class TypeInfo
    {
        private static readonly IDictionary<Type, IEnumerable<PropertyInfo>> _properties = new Dictionary<Type, IEnumerable<PropertyInfo>>();

        public static Type ConditionAttribute { get; } = typeof(ConditionAttribute);
        public static Type OutcomeAttribute { get; } = typeof(OutcomeAttribute);

        public static IEnumerable<PropertyInfo> Get(Type type)
        {
            if (!_properties.ContainsKey(type)) 
                _properties.Add(type, type.GetProperties());

            return _properties[type];
        }
    }
}
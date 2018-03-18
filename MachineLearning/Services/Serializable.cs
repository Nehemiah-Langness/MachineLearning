using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Domain.Contracts;

namespace Analysis.Services
{
    public abstract class Serializable<T> : Serializable
        where T : class
    {
        public readonly T Core;

        protected Serializable(T core)
        {
            Core = core;
        }

        public abstract T Set(IEnumerable<IKeyValue> values);
        public abstract IEnumerable<IKeyValue> Get();
    }

    public abstract class Serializable
    {
        protected static T SetValues<T>(T @object, IEnumerable<PropertyInfo> properties, IEnumerable<IKeyValue> values)
        {
            var propertiesToSet = properties.Join(values, property => property.Name, value => value.Key, (property, value) => new
            {
                property,
                value = value.Value
            });

            foreach (var propertyToSet in propertiesToSet)
                propertyToSet.property.SetValue(@object, propertyToSet.value);

            return @object;
        }

        protected static IEnumerable<IKeyValue> GetValues<T>(T @object, IEnumerable<PropertyInfo> properties)
        {
            return properties
                .Select(property => new KeyValuePair(property.Name, property.GetValue(@object)?.ToString()))
                .ToList();
        }
    }
}
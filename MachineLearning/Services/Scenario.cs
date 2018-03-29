using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Domain.Contracts;

namespace Analysis.Services
{
    internal class Scenario<T> : Serializable<T>
        where T : class
    {
        public Scenario(T core) : base(core) { }
        public override IEnumerable<PropertyInfo> Properties => Conditions;

        public static IEnumerable<PropertyInfo> Conditions { get; } = GetConditions();
        private static IEnumerable<PropertyInfo> GetConditions()
        {
            return TypeInfo<T>.Properties
                .Where(property => 
                    Attribute.IsDefined(property, TypeInfo.ConditionAttribute)
                    && property.CanRead
                    && property.CanWrite)
                .ToList();
        }
    }

    internal static class Scenario
    {
        public static T Set<T>(T @object, IEnumerable<IKeyValue> values) where T : class => Serializable.SetValues(@object, Scenario<T>.Conditions, values);

        public static Scenario<T> AsScenario<T> (this T @object) where T : class => new Scenario<T>(@object);
    }
}
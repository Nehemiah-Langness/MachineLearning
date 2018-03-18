using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Domain.Contracts;

namespace Analysis.Services
{
    public class Scenario<T> : Serializable<T>
        where T : class
    {
        public Scenario(T core) : base(core) { }

        public override T Set(IEnumerable<IKeyValue> values) => SetValues(Core, Conditions, values);
        public override IEnumerable<IKeyValue> Get() => GetValues(Core, Conditions);
        public static IEnumerable<PropertyInfo> Conditions { get; } = GetConditions();

        private static IEnumerable<PropertyInfo> GetConditions()
        {
            return TypeInfo<T>.Properties
                .Where(property => Attribute.IsDefined(property, TypeInfo.ConditionAttribute)
                                   && property.CanRead
                                   && property.CanWrite)
                .ToList();
        }

        public static T Set(T @object, IEnumerable<IKeyValue> values) => SetValues(@object, Conditions, values);
    }

    public static class Scenario
    {
        public static T Set<T>(T @object, IEnumerable<IKeyValue> values) where T : class => Scenario<T>.Set(@object, values);

        public static Scenario<T> AsScenario<T> (this T @object) where T : class => new Scenario<T>(@object);
    }
}
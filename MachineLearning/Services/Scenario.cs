using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Analysis.Services
{
    internal class Scenario<T> : Serializable<T>
        where T : class
    {
        public Scenario(T core) : base(core) { }
        public override IEnumerable<PropertyInfo> Properties => _conditions ?? (_conditions = GetConditions());

        private IEnumerable<PropertyInfo> _conditions;
        private IEnumerable<PropertyInfo> GetConditions()
        {
            return TypeInfo.Get(Core.GetType())
                .Where(property => 
                    Attribute.IsDefined(property, TypeInfo.ConditionAttribute)
                    && property.CanRead
                    && property.CanWrite)
                .ToList();
        }
    }

    internal static class Scenario
    {
        public static Scenario<T> AsScenario<T> (this T @object) where T : class => new Scenario<T>(@object);
    }
}
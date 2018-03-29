using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Analysis.Services
{
    internal class Result<T> : Serializable<T>
        where T : class
    {
        public Result(T core) : base(core) { }

        public override IEnumerable<PropertyInfo> Properties => _properties ?? (_properties = GetOutcomes());

        private IEnumerable<PropertyInfo> _properties;
        private IEnumerable<PropertyInfo> GetOutcomes()
        {
            return TypeInfo.Get(Core.GetType())
                .Where(property => 
                    Attribute.IsDefined(property, TypeInfo.OutcomeAttribute)
                    && property.CanRead
                    && property.CanWrite)
                .ToList();
        }
    }

    internal static class Result
    {
        public static Result<T> AsOutcome<T>(this T @object) where T : class => new Result<T>(@object);
    }
}
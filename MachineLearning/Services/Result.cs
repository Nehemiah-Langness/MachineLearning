using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Domain.Contracts;

namespace Analysis.Services
{
    internal class Result<T> : Serializable<T>
        where T : class
    {
        public Result(T core) : base(core) { }

        public override IEnumerable<PropertyInfo> Properties => Outcomes;

        public static IEnumerable<PropertyInfo> Outcomes { get; } = GetOutcomes();
        private static IEnumerable<PropertyInfo> GetOutcomes()
        {
            return TypeInfo<T>.Properties
                .Where(property => 
                    Attribute.IsDefined(property, TypeInfo.OutcomeAttribute)
                    && property.CanRead
                    && property.CanWrite)
                .ToList();
        }
    }

    internal static class Result
    {
        public static T Set<T>(T @object, IEnumerable<IKeyValue> values) where T : class => Serializable.SetValues(@object, Result<T>.Outcomes, values);

        public static Result<T> AsOutcome<T>(this T @object) where T : class => new Result<T>(@object);
    }
}
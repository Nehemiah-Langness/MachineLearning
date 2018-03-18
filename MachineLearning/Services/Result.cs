using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Domain.Contracts;

namespace Analysis.Services
{
    public class Result<T> : Serializable<T>
        where T : class
    {
        public Result(T core) : base(core) { }

        public override T Set(IEnumerable<IKeyValue> values) => SetValues(Core, Outcomes, values);
        public override IEnumerable<IKeyValue> Get() => GetValues(Core, Outcomes);
        public static IEnumerable<PropertyInfo> Outcomes { get; } = GetOutcomes();

        private static IEnumerable<PropertyInfo> GetOutcomes()
        {
            return TypeInfo<T>.Properties
                .Where(property => Attribute.IsDefined(property, TypeInfo.OutcomeAttribute)
                                && property.CanRead
                                && property.CanWrite)
                .ToList();
        }

        public static T Set(T @object, IEnumerable<IKeyValue> values) => SetValues(@object, Outcomes, values);
    }

    public static class Result
    {
        public static T Set<T>(T @object, IEnumerable<IKeyValue> values) where T : class => Result<T>.Set(@object, values);

        public static Result<T> AsOutcome<T>(this T @object) where T : class => new Result<T>(@object);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Contracts;

namespace Analysis.Services
{
    public static class Extensions
    {
        public static int Percent<T>(this IEnumerable<T> items, Func<T, bool> function)
        {
            var itemList = items.ToList();
            return itemList.Count(function) * 100 / itemList.Count;
        }

        public static IEnumerable<IKeyValue> GetScenario(this object scenario)
        {
            return scenario.AsScenario().Get();
        }

        public static object SetScenario(this object scenario, IEnumerable<IKeyValue> values)
        {
            return scenario.AsScenario().Set(values);
        }

        public static IEnumerable<IKeyValue> GetOutcome(this object outcome)
        {
            return outcome.AsOutcome().Get();
        }

        public static object SetOutcome(this object outcome, IEnumerable<IKeyValue> values)
        {
            return outcome.AsOutcome().Set(values);
        }
    }
}
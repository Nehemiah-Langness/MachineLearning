using System;
using System.Collections.Generic;
using System.Linq;

namespace Analysis.Services
{
    public static class Extensions
    {
        public static T Max<T>(this IEnumerable<T> list, T defaultValue)
        {
            if (list == null || !list.Any()) return defaultValue;
            return list.Max();
        }

        public static TMax Max<T, TMax>(this IEnumerable<T> list, Func<T, TMax> function, TMax defaultValue)
        {
            if (list == null || !list.Any()) return defaultValue;
            return list.Max(function);
        }

        public static int Percent<T>(this IEnumerable<T> items, Func<T, bool> function)
        {
            var itemList = items.ToList();
            return itemList.Count(function) * 100 / itemList.Count;
        }
    }
}
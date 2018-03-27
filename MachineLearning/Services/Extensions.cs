using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Contracts;

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

        public static int GetWieght(this ResultStatus status)
        {
            switch (status) {
                case ResultStatus.Success:
                    return 2;
                case ResultStatus.Inconclusive:
                    return 1;
                case ResultStatus.Failure:
                    return 0;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }
    }
}
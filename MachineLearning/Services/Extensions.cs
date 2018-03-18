using System;
using Domain.Contracts;

namespace Analysis.Services
{
    public static class Extensions
    {
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
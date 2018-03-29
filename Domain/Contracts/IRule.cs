using System.Collections.Generic;

namespace Domain.Contracts
{
    public interface IRule
    {
        int Score { get; }
        int Attempts { get; }
        IEnumerable<IKeyValue> Conditions { get; }
        IEnumerable<IKeyValue> Outcomes { get; }

        void AddResult(ResultStatus status);
    }
}
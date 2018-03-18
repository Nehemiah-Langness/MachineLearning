using System.Collections.Generic;

namespace Domain.Contracts
{
    public interface IRule
    {
        ResultStatus Result { get; }
        IEnumerable<IKeyValue> Conditions { get; }
        IEnumerable<IKeyValue> Outcomes { get; }
    }
}
using System.Collections.Generic;

namespace Domain.Contracts
{
    public interface IAnalyticSystem<TScenario, in TResult>
        where TResult : class, IResult<TScenario>, new()
        where TScenario : class
    {
        IEnumerable<IRule> GetHistory();
        IEnumerable<TScenario> GetTests();
        ResultStatus IsSuccess(TScenario scenario, TResult result);
    }
}
namespace Domain.Contracts
{
    public interface IAnalyticResult<out TScenario, out TResult>
        where TResult : class, IResult<TScenario>, new()
        where TScenario : class
    {
        TScenario Scenario { get; }
        TResult Outcome { get; }
        ResultStatus Result { get; }
        DerivationMethod Method { get; }
    }
}
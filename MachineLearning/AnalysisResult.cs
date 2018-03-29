using Domain.Contracts;

namespace Analysis
{
    public class AnalysisResult<TScenario, TResult> : IAnalyticResult<TScenario, TResult> 
        where TResult : class, IResult<TScenario>, new() 
        where TScenario : class
    {
        public AnalysisResult(TScenario scenario, TResult outcome, DerivationMethod method, ResultStatus result)
        {
            Scenario = scenario;
            Outcome = outcome;
            Result = result;
            Method = method;
        }

        public TScenario Scenario { get; }
        public TResult Outcome { get; }
        public ResultStatus Result { get; }
        public DerivationMethod Method { get; }

        public override string ToString()
        {
            return $"{Scenario}\nA {Method} approach was taken\n{Outcome}\nIt was {Result}";
        }
    }
}
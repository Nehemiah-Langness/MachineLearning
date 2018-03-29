using System.Collections.Generic;
using System.Linq;
using Analysis.Services;
using Domain.Contracts;

namespace Analysis
{
    public class AnalysisService<TScenario, TResult>
        where TResult : class, IResult<TScenario>, new()
        where TScenario : class
    {
        private readonly IAnalyticSystem<TScenario, TResult> _system;
        private readonly ICollection<IRule> _history;

        public AnalysisService(IAnalyticSystem<TScenario, TResult> system)
        {
            _system = system;
            _history = _system.GetHistory().ToList();
        }

        public IEnumerable<IAnalyticResult<TScenario, TResult>> Run()
        {
            return _system.GetTests().Select(RunSingle);
        }

        public IAnalyticResult<TScenario, TResult> RunSingle(TScenario scenario)
        {
            var bestChoice = GetBestChoice(scenario.GetScenario());
            var method = bestChoice == null ? DerivationMethod.Heuristic : DerivationMethod.Historical;

            var outcome = method == DerivationMethod.Historical
                    ? new TResult().AsOutcome().Set(bestChoice.Rule.Outcomes)
                    : (TResult)new TResult().Heuristic(scenario);

            var result = _system.IsSuccess(scenario, outcome);

            SaveTest(scenario, bestChoice, result, outcome);

            return new AnalysisResult<TScenario, TResult>(scenario, outcome, method, result);
        }

        private AnalysisChoice GetBestChoice(IEnumerable<IKeyValue> scenario)
        {
            var choices = _history.Select(test => new AnalysisChoice
            {
                MatchRate = test.Conditions
                    .Join(scenario, c => c.Key, s => s.Key, (history, current) => history.Value == current.Value)
                    .Percent(m => m),
                SuccessRate = test.SuccessRate,
                Rule = test
            }).ToList();

            return choices
                .OrderByDescending(b => b.MatchRate)
                .ThenByDescending(b => b.SuccessRate)
                .FirstOrDefault(b => b.IsGoodChoice());
        }

        private void SaveTest(TScenario scenario, AnalysisChoice bestChoice, ResultStatus result, TResult outcome)
        {
            if (bestChoice != null && bestChoice.MatchRate == 100)
                bestChoice.Rule.AddResult(result);
            else
                _history.Add(new AnalysisTest<TScenario, TResult>(scenario, outcome).AddResult(result));
        }
    }
}

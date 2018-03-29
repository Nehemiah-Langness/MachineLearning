using System;
using System.Collections.Generic;
using System.Linq;
using Analysis.Services;
using Domain;
using Domain.Contracts;

namespace Analysis
{
    public class AnalysisService
    {
        private readonly ICollection<IRule> _history;

        public AnalysisService(IEnumerable<IRule> tests)
        {
            _history = tests.ToList();
        }

        public AnalysisResult<TResult> Run<TScenario, TResult>(TScenario scenario, Func<TScenario, TResult, ResultStatus> success)
            where TResult : class, IResult<TScenario>, new()
            where TScenario : class
        {
            var bestChoice = GetBestChoice(scenario.AsScenario());
            var outcome = bestChoice != null
                    ? Result.Set(new TResult(), bestChoice.Rule.Outcomes)
                    : (TResult)new TResult().Heuristic(scenario);

            var result = success(scenario, outcome);

            SaveTest(scenario, bestChoice, result, outcome);

            return new AnalysisResult<TResult>(outcome, result);
        }

        private AnalysisChoice GetBestChoice<TScenario>(Serializable<TScenario> scenario) where TScenario : class
        {
            var choices = _history.Select(test => new AnalysisChoice
            {
                MatchRate = test.Conditions
                    .Join(scenario.Get(), c => c.Key, s => s.Key, (history, current) => history.Value == current.Value)
                    .Percent(m => m),
                SuccessRate = test.Score * 100 / (test.Attempts * ResultStatus.Success.GetWieght()),
                Rule = test
            }).ToList();

            return choices
                .OrderByDescending(b => b.MatchRate)
                .ThenByDescending(b => b.SuccessRate)
                .FirstOrDefault(b => b.IsGoodChoice());
        }

        private void SaveTest<TScenario, TResult>(TScenario scenario, AnalysisChoice bestChoice, ResultStatus result, TResult outcome)
            where TResult : class, IResult<TScenario>, new()
            where TScenario : class
        {
            if (bestChoice != null && bestChoice.MatchRate == 100)
                bestChoice.Rule.AddResult(result);
            else
                _history.Add(new Test<TScenario, TResult>(scenario, outcome, result).AddResult(result));
        }
    }
}

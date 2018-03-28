using System;
using System.Collections.Generic;
using System.Linq;
using Analysis.Services;
using Domain.Contracts;

namespace Analysis
{
    public class AnalysisService
    {
        private readonly ICollection<Test> _tests;

        public AnalysisService(IEnumerable<Test> tests)
        {
            _tests = tests.ToList();
        }

        public AnalysisResult<TResult> Run<TScenario, TResult>(TScenario instance, Func<TScenario, TResult, ResultStatus> success)
            where TResult : class, IResult<TScenario>, new()
            where TScenario : class
        {
            var scenario = instance.AsScenario();

            var allChoices = _tests.Select(test => new
            {
                outcomes = test.Outcomes,
                matches = test.Conditions.Join(scenario.Get(), c => c.Key, s => s.Key,
                    (history, current) => history.Value == current.Value),
                success = test.Result
            });

            //Log.Write(string.Join("\n", allChoices.Select(ac => $"{ac.success} : {ac.matches.Count(m => m)} : {string.Join("-", ac.outcomes.Select(o => $"{o.Key}:{o.Value}"))}")));

            var bestChoices = allChoices.Select(x => new
            {
                x.outcomes,
                percentMatch = (int)Math.Round((double)x.matches.Count(m => m) * 100 / x.matches.Count()),
                x.success
            }).OrderByDescending(x => x.percentMatch);

            Log.Write(string.Join("\n", bestChoices.Select(ac => $"{ac.success} : {ac.percentMatch} : {string.Join("-", ac.outcomes.Select(o => $"{o.Key}:{o.Value}"))}")));


            var maxPercentMatch = bestChoices.Max(be => be.percentMatch, 0);
            var bestChoice = bestChoices.Where(b => b.percentMatch == maxPercentMatch)
                .OrderByDescending(b => b.success.GetWieght()).FirstOrDefault();

            var outcome = (bestChoice == null || bestChoice.success == ResultStatus.Failure || bestChoice.percentMatch < 75)
                    ? (TResult)new TResult().Heuristic(instance)
                    : Result.Set(new TResult(), bestChoice.outcomes);

            var result = success(instance, outcome);
            _tests.Add(new Test<TScenario, TResult>(instance, outcome, result));

            return new AnalysisResult<TResult>(outcome, result);
        }
    }
}

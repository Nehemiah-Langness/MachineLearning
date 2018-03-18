using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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

            var bestChoices = _tests.Select(test => new
            {
                outcomes = test.Outcomes,
                matches = test.Conditions.Join(scenario.Get(), c => c.Key, s => s.Key,
                    (history, current) => history.Value == current.Value),
                success = test.Result
            });
            var next = bestChoices.Select(x => new
            {
                x.outcomes,
                percentMatch = (int)Math.Round((x.matches.Count(m => m) / (double)x.matches.Count()) * 100),
                x.success
            }).OrderByDescending(x => x.percentMatch);

            var max = next.Max(be => be.percentMatch);
            var bestChoice = next.Where(b => b.percentMatch == max).OrderByDescending(b => b.success.GetWieght()).FirstOrDefault();

            var outcome =
                (bestChoice == null || bestChoice.success == ResultStatus.Failure)
                    ? (TResult)new TResult().Heuristic(instance)
                    : Result.Set(new TResult(), bestChoice.outcomes);


            var result = success(instance, outcome);
            _tests.Add(new Test<TScenario, TResult>(instance, outcome, result));

            return new AnalysisResult<TResult>(outcome, result);
        }
    }
}

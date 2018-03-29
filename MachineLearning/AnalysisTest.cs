using System.Collections.Generic;
using Analysis.Services;
using Domain;
using Domain.Contracts;

namespace Analysis
{
    internal class AnalysisTest : IRule
    {
        private int Score { get; set; }
        private int MaxScore { get; set; }
        public IEnumerable<IKeyValue> Conditions { get; protected set; }
        public IEnumerable<IKeyValue> Outcomes { get; protected set; }

        public int SuccessRate => MaxScore == 0 ? 0 : Score * 100 / MaxScore;

        public void AddResult(ResultStatus status)
        {
            Score += status.GetWieght();
            MaxScore += ResultStatus.Success.GetWieght();
        }
    }

    internal class AnalysisTest<TScenario, TResult> : AnalysisTest
        where TScenario : class
        where TResult : class
    {
        public AnalysisTest(TScenario scenario, TResult outcome)
        {
            Conditions = scenario.AsScenario().Get();
            Outcomes = outcome.AsOutcome().Get();
        }

        public new AnalysisTest<TScenario, TResult> AddResult(ResultStatus status)
        {
            base.AddResult(status);
            return this;
        }
    }
}
using System.Collections.Generic;
using Analysis.Services;
using Domain;
using Domain.Contracts;

namespace Analysis
{
    public class Test : IRule
    {
        public Test(ResultStatus result)
        {
            Result = result;
        }

        public int Score { get; set; }
        public int Attempts { get; set; }
        public ResultStatus Result { get; }
        public IEnumerable<IKeyValue> Conditions { get; protected set; }
        public IEnumerable<IKeyValue> Outcomes { get; protected set; }

        public void AddResult(ResultStatus status)
        {
            Attempts++;
            Score += status.GetWieght();
        }
    }

    public class Test<TScenario, TResult> : Test 
        where TScenario : class 
        where TResult : class
    {
        public Test(TScenario scenario, TResult outcome, ResultStatus result) : base(result)
        {
            Conditions = scenario.AsScenario().Get();
            Outcomes = outcome.AsOutcome().Get();
        }

        public new Test<TScenario, TResult> AddResult(ResultStatus status)
        {
            base.AddResult(status);
            return this;
        }
    }
}
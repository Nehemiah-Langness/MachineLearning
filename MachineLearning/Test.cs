using System.Collections.Generic;
using Analysis.Services;
using Domain.Contracts;

namespace Analysis
{
    public class Test : IRule
    {
        public Test(ResultStatus result)
        {
            Result = result;
        }

        public ResultStatus Result { get; }
        public IEnumerable<IKeyValue> Conditions { get; protected set; }
        public IEnumerable<IKeyValue> Outcomes { get; protected set; }
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
    }
}
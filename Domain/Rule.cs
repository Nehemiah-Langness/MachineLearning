using System.Collections.Generic;
using Domain.Base;
using Domain.Contracts;

namespace Domain
{
    public class Rule : Entity, IRule
    {
        public int Score { get; set; }
        public int MaxScore { get; set; }

        public ICollection<Condition> Conditions { get; set; } = new HashSet<Condition>();
        public ICollection<Outcomes> Outcomes { get; set; } = new HashSet<Outcomes>();

        int IRule.SuccessRate => MaxScore == 0 ? 0 : Score * 100 / MaxScore;
        IEnumerable<IKeyValue> IRule.Conditions => Conditions;
        IEnumerable<IKeyValue> IRule.Outcomes => Outcomes;
        void IRule.AddResult(ResultStatus status)
        {
            Score += status.GetWieght();
            MaxScore += ResultStatus.Success.GetWieght();
        }
    }
}
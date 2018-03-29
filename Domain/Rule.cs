using System.Collections.Generic;
using System.Linq;
using Domain.Base;
using Domain.Contracts;

namespace Domain
{
    public class Rule : Entity, IRule
    {
        public int Score { get; set; }
        public int Attempts { get; set; }
        public ICollection<Condition> Conditions { get; set; } = new HashSet<Condition>();
        public ICollection<Outcomes> Outcomes { get; set; } = new HashSet<Outcomes>();
        public ResultStatus Result { get; set; }

        IEnumerable<IKeyValue> IRule.Conditions => Conditions.Select(c => (IKeyValue) c);
        IEnumerable<IKeyValue> IRule.Outcomes => Outcomes.Select(o => (IKeyValue)o);

        void IRule.AddResult(ResultStatus status)
        {
            Attempts++;
            Score += status.GetWieght();
        }
    }
}
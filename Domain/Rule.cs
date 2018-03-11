using System.Collections.Generic;
using Domain.Base;

namespace Domain
{
    public class Rule : Entity
    {
        public ICollection<Condition> Conditions { get; set; } = new HashSet<Condition>();
        public ICollection<Outcomes> Outcomes { get; set; } = new HashSet<Outcomes>();
        public ResultStatus Result { get; set; }
    }
}
using Domain.Base;
using Domain.Contracts;

namespace Domain
{
    public class Condition : Entity, IKeyValue
    {
        public string Property { get; set; }
        public string Value { get; set; }

        public int RuleId { get; set; }
        public Rule Rule { get; set; }

        string IKeyValue.Key => Property;
    }
}


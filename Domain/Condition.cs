using Domain.Base;

namespace Domain
{
    public class Condition : Entity
    {
        public string Property { get; set; }
        public string Value { get; set; }

        public int RuleId { get; set; }
        public Rule Rule { get; set; }
    }
}

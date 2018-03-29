using System;
using Analysis.Attributes;
using Domain.Contracts;

namespace MachineLearningApp
{
    public class Meal : IResult<Person>
    {
        public bool WillEat { get; set; }
        public FoodType? RestrauntType { get; set; }

        [Outcome]
        public string OutcomeWillEat
        {
            get => WillEat.ToString();
            set => WillEat = Convert.ToBoolean(value);
        }

        [Outcome]
        public string OutcomeRestrauntType
        {
            get => ((int?)RestrauntType)?.ToString();
            set => RestrauntType = value == null ? null : (FoodType?)Convert.ToInt32(value);
        }

        public IResult<Person> Heuristic(Person scenario)
        {
            if (scenario.IsHungry)
                WillEat = true;

            RestrauntType = RandomService.Get(0, 100) < 30 ? null : (FoodType?)RandomService.Get(0, 4);

            return this;
        }

        public override string ToString()
        {
            return $"They decided to {(WillEat ? "" : "not ")}eat{(RestrauntType != null ? $" {RestrauntType}" : "")}";
        }
    }
}
using System;
using Analysis.Attributes;

namespace MachineLearningApp
{
    public class Person
    {
        public string Name { get; set; }

        public bool IsHungry { get; set; }
        public FoodType? HungryFor { get; set; }

        [Condition]
        public string ConditionIsHungry
        {
            get => IsHungry.ToString();
            set => IsHungry = Convert.ToBoolean(value);
        }

        [Condition]
        public string ConditionHungryFor
        {
            get => ((int?)HungryFor)?.ToString();
            set => HungryFor = value == null ? null : (FoodType?)Convert.ToInt32(value);
        }

        public override string ToString()
        {
            return $"{Name} is {(IsHungry ? "" : "not ")}hungry{(HungryFor != null ? $" for {HungryFor}" : "")}";
        }
    }
}
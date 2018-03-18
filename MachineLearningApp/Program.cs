using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Analysis;
using Analysis.Attributes;
using Domain;
using Domain.Contracts;

namespace MachineLearningApp
{
    internal static class Program
    {
        public static readonly Random Random = new Random();

        internal static void Main(string[] args)
        {
            var service = new AnalysisService(SeedRules());
            foreach (var person in GetPeople())
            {
                Console.WriteLine(person);
                var result = service.Run<Person, Meal>(person, IsSuccess);
                Console.WriteLine(result.Outcome);

                switch (result.Result)
                {
                    case ResultStatus.Success:
                        Console.WriteLine("It was a good choice");
                        break;
                    case ResultStatus.Failure:
                        Console.WriteLine("It was a bad choice");
                        break;
                    case ResultStatus.Inconclusive:
                        Console.WriteLine("They are indifferent on their decision");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                Console.WriteLine();
            }

            Console.WriteLine("Analysis Complete");
            Console.ReadLine();
        }

        private static ResultStatus IsSuccess(Person person, Meal meal)
        {
            var personIsNotHungry = !person.IsHungry && person.HungryFor == null;
            var personDidNotEat = !meal.WillEat && meal.RestrauntType == null;

            if (personDidNotEat)
            {
                if (personIsNotHungry)
                    return ResultStatus.Success;
                return ResultStatus.Failure;
            }

            if (personIsNotHungry) return ResultStatus.Failure;

            var personWantsSpecificFood = person.IsHungry && person.HungryFor.HasValue;
            var personDoesNotWantSpecificFood = !person.IsHungry && person.HungryFor.HasValue;

            var personAteSpecificFood = meal.WillEat && meal.RestrauntType.HasValue;
            var personDidNotEatSpecificFood = !meal.WillEat && meal.RestrauntType.HasValue;

            if (personWantsSpecificFood)
            {
                if (personAteSpecificFood)
                {
                    if (person.HungryFor == meal.RestrauntType)
                        return ResultStatus.Success;
                    return ResultStatus.Failure;
                }

                if (personDidNotEatSpecificFood)
                {
                    if (person.HungryFor == meal.RestrauntType)
                        return ResultStatus.Failure;
                    return ResultStatus.Inconclusive;
                }

                return ResultStatus.Inconclusive;
            }

            if (personDoesNotWantSpecificFood)
            {
                if (personAteSpecificFood)
                {
                    if (person.HungryFor == meal.RestrauntType)
                        return ResultStatus.Failure;
                    return ResultStatus.Success;
                }

                if (personDidNotEatSpecificFood)
                {
                    if (person.HungryFor == meal.RestrauntType)
                        return ResultStatus.Success;
                    return ResultStatus.Inconclusive;
                }

                return ResultStatus.Inconclusive;
            }

            return ResultStatus.Success;
        }

        private static IEnumerable<Person> GetPeople()
        {
            string[] names = {
                "James",
                "John",
                "Robert",
                "Michael",
                "William",
                "David",
                "Richard",
                "Charles",
                "Joseph",
                "Thomas",
                "Mary",
                "Patricia",
                "Linda",
                "Barbara",
                "Elizabeth",
                "Jennifer",
                "Maria",
                "Susan",
                "Margaret",
                "Dorothy"
            };

            return Enumerable.Repeat(0, 20).Select(i =>
                new Person
                {
                    Name = names[Random.Next(0, names.Length - 1)],
                    IsHungry = Random.Next(0, 100) < 50,
                    HungryFor = Random.Next(0, 100) < 30 ? null : (FoodType?)Random.Next(0, 4)
                }
            ).ToList();

        }

        private static IEnumerable<Test> SeedRules()
        {
            return new List<Test>
            {
                new Test<Person,Meal>(
                    new Person
                    {
                        IsHungry = true,
                        HungryFor = FoodType.Chinese
                    },
                    new Meal
                    {
                        WillEat = true,
                        RestrauntType = FoodType.Chinese
                    }, ResultStatus.Success),
                new Test<Person,Meal>(
                    new Person
                    {
                        IsHungry = false,
                    },
                    new Meal
                    {
                        WillEat = false,
                    }, ResultStatus.Success),
                new Test<Person,Meal>(
                    new Person
                    {
                        IsHungry = true,
                        HungryFor = FoodType.Chinese
                    },
                    new Meal
                    {
                        WillEat = true,
                        RestrauntType = FoodType.Mexican
                    }, ResultStatus.Failure),
                new Test<Person,Meal>(
                    new Person
                    {
                        IsHungry = true,
                        HungryFor = FoodType.Pizza
                    },
                    new Meal
                    {
                        WillEat = true,
                        RestrauntType = FoodType.Pizza
                    }, ResultStatus.Success),
            };
        }
    }

    public enum FoodType
    {
        Chinese,
        Pizza,
        Mexican,
        Burgers,
        Deli
    }

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

            RestrauntType = Program.Random.Next(0, 100) < 30 ? null : (FoodType?)Program.Random.Next(0, 4);

            return this;
        }

        public override string ToString()
        {
            return $"They decided to {(WillEat ? "" : "not ")}eat{(RestrauntType != null ? $" {RestrauntType}" : "")}";
        }
    }
}

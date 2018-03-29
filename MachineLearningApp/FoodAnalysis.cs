using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Contracts;

namespace MachineLearningApp
{
    internal class FoodAnalysis : IAnalyticSystem<Person, Meal>
    {
        private static readonly Random Random = new Random();

        public IEnumerable<IRule> GetHistory()
        {
            return new List<IRule>
            {
                
            };
        }  

        public IEnumerable<Person> GetTests()
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

        public ResultStatus IsSuccess(Person person, Meal meal)
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
    }
}
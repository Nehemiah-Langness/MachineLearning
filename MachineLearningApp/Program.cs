﻿using System;
using System.Linq;
using Analysis;
using MachineLearningApp.Services;

namespace MachineLearningApp
{
    internal static class Program
    {
        internal static void Main(string[] args)
        {
            var analysisSystem = new FoodAnalysis();
            var service = new AnalysisService<Person, Meal>(analysisSystem);

            var results = service.Run();
            foreach (var result in results)
            {
                //Console.WriteLine(result.Scenario);
                //Console.WriteLine(result.Outcome);
                //switch (result.Result)
                //{
                //    case ResultStatus.Success:
                //        Console.WriteLine("It was a good choice");
                //        break;
                //    case ResultStatus.Failure:
                //        Console.WriteLine("It was a bad choice");
                //        break;
                //    case ResultStatus.Inconclusive:
                //        Console.WriteLine("They are indifferent on their decision");
                //        break;
                //    default:
                //        throw new ArgumentOutOfRangeException();
                //}
                Log.Write(result);
                Log.Write(string.Join(string.Empty, Enumerable.Repeat("-", 10)));
            }

            Console.WriteLine("Analysis Complete");
            Console.ReadLine();
        }
    }
}

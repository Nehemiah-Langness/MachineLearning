using System;

namespace MachineLearningApp
{
    public static class RandomService
    {
        private static readonly Random RandomNumberGenerator = new Random();

        public static int Get(int min, int max) => RandomNumberGenerator.Next(min, max);
    }
}
using System;

namespace MachineLearningApp.Services
{
    public static class Log
    {
        public static void Write(object log)
        {
            Console.WriteLine(log.ToString());
        }
    }
}
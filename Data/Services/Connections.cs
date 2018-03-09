namespace Data.Services
{
    internal class Connections
    {
        public const string Test = "test-app";
        public const string Production = "prod-app";

        public static string Get(bool test)
        {
            return test ? Test : Production;
        }
    }
}
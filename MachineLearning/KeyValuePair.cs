using Domain.Contracts;

namespace Analysis
{
    public class KeyValuePair : IKeyValue
    {
        public string Key { get; }
        public string Value { get; }

        public KeyValuePair(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
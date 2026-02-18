namespace DotSerial.Benchmarks.Helpers
{
    internal class DictionaryClass
    {
        public Dictionary<int, int>? Value0 { get; set; }

        internal static DictionaryClass Create(int numOfDicItems)
        {
            var result = new DictionaryClass
            {
                Value0 = []
            };

            for (int i = 0; i < numOfDicItems; i++)
            {
                result.Value0.Add(i, i + 1);
            }

            return result;

        }
    }
}

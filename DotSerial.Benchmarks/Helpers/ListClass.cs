namespace DotSerial.Benchmarks.Helpers
{
    public class ListClass
    {
        public List<List<int>>? Value0 { get; set; }

        internal static ListClass Create(int numOfList, int numOfListItems)
        {
            var result = new ListClass
            {
                Value0 = []
            };

            for (int i = 0; i < numOfList; i++)
            {
                List<int> tmp = [];
                for (int j = 0; j < numOfListItems; j++)
                {
                    tmp.Add(42);
                }
                result.Value0.Add(tmp);
            }

            return result;

        }
    }
}

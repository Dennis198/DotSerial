using BenchmarkDotNet.Attributes;
using DotSerial.Benchmarks.Helpers;
using DotSerial.Toon;

namespace DotSerial.Benchmarks.Benchmarks.Toon
{
    [MemoryDiagnoser]
    public class ToonWriterBenchmarks
    {
        private DSToonNode _nodePrimitve;
        private DSToonNode _nodeList;
        private DSToonNode _nodeDictionary;

        [GlobalSetup]
        public void Setup()
        {
            var primitiveClass = PrimitiveClass.Create();
            _nodePrimitve = DSToonNode.ToNode(primitiveClass);

            var listClass = ListClass.Create(50, 50);
            _nodeList = DSToonNode.ToNode(listClass);

            var dicClass = DictionaryClass.Create(50);
            _nodeDictionary = DSToonNode.ToNode(dicClass);
        }

        [Benchmark]
        public string PrimitiveWriteTest()
        {
            string result = _nodePrimitve.Stringify();
            return result;
        }

        [Benchmark]
        public string ListWriteTest()
        {
            string result = _nodeList.Stringify();
            return result;
        }

        [Benchmark]
        public string DictionaryWriteTest()
        {
            string result = _nodeDictionary.Stringify();
            return result;
        }
    }
}

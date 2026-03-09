using BenchmarkDotNet.Attributes;
using DotSerial.Benchmarks.Helpers;
using DotSerial.Json;

namespace DotSerial.Benchmarks.Benchmarks.Json
{
    [MemoryDiagnoser]
    public class JsonWriterBenchmarks
    {
        private DSJsonNode? _nodePrimitve;
        private DSJsonNode? _nodeList;
        private DSJsonNode? _nodeDictionary;

        [GlobalSetup]
        public void Setup()
        {
            var primitiveClass = PrimitiveClass.Create();
            _nodePrimitve = DSJsonNode.ToNode(primitiveClass);

            var listClass = ListClass.Create(2, 50);
            _nodeList = DSJsonNode.ToNode(listClass);

            var dicClass = DictionaryClass.Create(50);
            _nodeDictionary = DSJsonNode.ToNode(dicClass);
        }

        [Benchmark]
        public string PrimitiveWriteTest()
        {
            if (null == _nodePrimitve)
            {
                throw new InvalidOperationException("Node is null");
            }
            string result = _nodePrimitve.Stringify();
            return result;
        }

        [Benchmark]
        public string ListWriteTest()
        {
            if (null == _nodeList)
            {
                throw new InvalidOperationException("Node is null");
            }
            string result = _nodeList.Stringify();
            return result;
        }

        [Benchmark]
        public string DictionaryWriteTest()
        {
            if (null == _nodeDictionary)
            {
                throw new InvalidOperationException("Node is null");
            }
            string result = _nodeDictionary.Stringify();
            return result;
        }
    }
}

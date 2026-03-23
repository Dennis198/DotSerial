using BenchmarkDotNet.Attributes;
using DotSerial.Benchmarks.Helpers;

namespace DotSerial.Benchmarks.Benchmarks.Yaml
{
    [MemoryDiagnoser]
    public class YamlWriterBenchmarks
    {
        private readonly SerializeStrategy _strategy = SerializeStrategy.Yaml;
        private DSNode? _nodePrimitve;
        private DSNode? _nodeList;
        private DSNode? _nodeDictionary;

        [GlobalSetup]
        public void Setup()
        {
            var primitiveClass = PrimitiveClass.Create();
            _nodePrimitve = DSNode.ToNode(primitiveClass, _strategy);

            var listClass = ListClass.Create(2, 50);
            _nodeList = DSNode.ToNode(listClass, _strategy);

            var dicClass = DictionaryClass.Create(50);
            _nodeDictionary = DSNode.ToNode(dicClass, _strategy);
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

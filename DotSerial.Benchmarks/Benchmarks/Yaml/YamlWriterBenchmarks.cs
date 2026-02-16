using BenchmarkDotNet.Attributes;
using DotSerial.Benchmarks.Helpers;
using DotSerial.Yaml;

namespace DotSerial.Benchmarks.Benchmarks.Yaml
{
    [MemoryDiagnoser]
    public class YamlWriterBenchmarks
    {
        private DSYamlNode _nodePrimitve;
        private DSYamlNode _nodeList;
        private DSYamlNode _nodeDictionary;

        [GlobalSetup]
        public void Setup()
        {
            var primitiveClass = PrimitiveClass.Create();
            _nodePrimitve = DSYamlNode.ToNode(primitiveClass);

            var listClass = ListClass.Create(50, 50);
            _nodeList = DSYamlNode.ToNode(listClass);

            var dicClass = DictionaryClass.Create(50);
            _nodeDictionary = DSYamlNode.ToNode(dicClass);
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

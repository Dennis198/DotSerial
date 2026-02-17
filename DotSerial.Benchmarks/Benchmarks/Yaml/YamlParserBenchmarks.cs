using BenchmarkDotNet.Attributes;
using DotSerial.Benchmarks.Helpers;
using DotSerial.Yaml;

namespace DotSerial.Benchmarks.Benchmarks.Yaml
{
    [MemoryDiagnoser]
    public class YamlParserBenchmarks
    {
        private string _stringPrimitive;
        private string _stringList;
        private string _stringDictionary;

        [GlobalSetup]
        public void Setup()
        {
            var primitiveClass = PrimitiveClass.Create();
            var nodePrimitive = DSYamlNode.ToNode(primitiveClass);
            _stringPrimitive = nodePrimitive.Stringify();

            var listClass = ListClass.Create(50, 50);
            var nodeList = DSYamlNode.ToNode(listClass);
            _stringList = nodeList.Stringify();

            var dicClass = DictionaryClass.Create(50);
            var nodeDictionary = DSYamlNode.ToNode(dicClass);
            _stringDictionary = nodeDictionary.Stringify();
        }

        [Benchmark]
        public DSYamlNode PrimitiveParseTest()
        {
            var result = DSYamlNode.FromString(_stringPrimitive);
            return result;
        }

        [Benchmark]
        public DSYamlNode ListParseTest()
        {
            var result = DSYamlNode.FromString(_stringList);
            return result;
        }

        [Benchmark]
        public DSYamlNode DictionaryParseTest()
        {
            var result = DSYamlNode.FromString(_stringDictionary);
            return result;
        }
    }
}

using BenchmarkDotNet.Attributes;
using DotSerial.Benchmarks.Helpers;

namespace DotSerial.Benchmarks.Benchmarks.Yaml
{
    [MemoryDiagnoser]
    public class YamlParserBenchmarks
    {
        private readonly StategyType _strategy = StategyType.Yaml;
        private string? _stringPrimitive;
        private string? _stringList;
        private string? _stringDictionary;

        [GlobalSetup]
        public void Setup()
        {
            var primitiveClass = PrimitiveClass.Create();
            var nodePrimitive = DSNode.ToNode(primitiveClass, _strategy);
            _stringPrimitive = nodePrimitive.Stringify(_strategy);

            var listClass = ListClass.Create(2, 50);
            var nodeList = DSNode.ToNode(listClass, _strategy);
            _stringList = nodeList.Stringify(_strategy);

            var dicClass = DictionaryClass.Create(50);
            var nodeDictionary = DSNode.ToNode(dicClass, _strategy);
            _stringDictionary = nodeDictionary.Stringify(_strategy);
        }

        [Benchmark]
        public DSNode PrimitiveParseTest()
        {
            if (string.IsNullOrWhiteSpace(_stringPrimitive))
            {
                throw new InvalidOperationException("String is null or empty");
            }
            var result = DSNode.FromString(_stringPrimitive, _strategy);
            return result;
        }

        [Benchmark]
        public DSNode ListParseTest()
        {
            if (string.IsNullOrWhiteSpace(_stringList))
            {
                throw new InvalidOperationException("String is null or empty");
            }
            var result = DSNode.FromString(_stringList, _strategy);
            return result;
        }

        [Benchmark]
        public DSNode DictionaryParseTest()
        {
            if (string.IsNullOrWhiteSpace(_stringDictionary))
            {
                throw new InvalidOperationException("String is null or empty");
            }
            var result = DSNode.FromString(_stringDictionary, _strategy);
            return result;
        }
    }
}

using BenchmarkDotNet.Attributes;
using DotSerial.Benchmarks.Helpers;
using DotSerial.Json;

namespace DotSerial.Benchmarks.Benchmarks.Json
{
    [MemoryDiagnoser]
    public class JsonParserBenchmarks
    {
        private string _stringPrimitive;
        private string _stringList;
        private string _stringDictionary;

        [GlobalSetup]
        public void Setup()
        {
            var primitiveClass = PrimitiveClass.Create();
            var nodePrimitive = DSJsonNode.ToNode(primitiveClass);
            _stringPrimitive = nodePrimitive.Stringify();

            var listClass = ListClass.Create(50, 50);
            var nodeList = DSJsonNode.ToNode(listClass);
            _stringList = nodeList.Stringify();

            var dicClass = DictionaryClass.Create(50);
            var nodeDictionary = DSJsonNode.ToNode(dicClass);
            _stringDictionary = nodeDictionary.Stringify();
        }

        [Benchmark]
        public DSJsonNode PrimitiveParseTest()
        {
            var result = DSJsonNode.FromString(_stringPrimitive);
            return result;
        }

        [Benchmark]
        public DSJsonNode ListParseTest()
        {
            var result = DSJsonNode.FromString(_stringList);
            return result;
        }

        [Benchmark]
        public DSJsonNode DictionaryParseTest()
        {
            var result = DSJsonNode.FromString(_stringDictionary);
            return result;
        }
    }
}

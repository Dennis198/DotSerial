using BenchmarkDotNet.Attributes;
using DotSerial.Benchmarks.Helpers;
using DotSerial.Toon;

namespace DotSerial.Benchmarks.Benchmarks.Toon
{
    [MemoryDiagnoser]
    public class ToonParserBenchmarks
    {
        private string _stringPrimitive;
        private string _stringList;
        private string _stringDictionary;

        [GlobalSetup]
        public void Setup()
        {
            var primitiveClass = PrimitiveClass.Create();
            var nodePrimitive = DSToonNode.ToNode(primitiveClass);
            _stringPrimitive = nodePrimitive.Stringify();

            var listClass = ListClass.Create(50, 50);
            var nodeList = DSToonNode.ToNode(listClass);
            _stringList = nodeList.Stringify();

            var dicClass = DictionaryClass.Create(50);
            var nodeDictionary = DSToonNode.ToNode(dicClass);
            _stringDictionary = nodeDictionary.Stringify();
        }

        [Benchmark]
        public DSToonNode PrimitiveParseTest()
        {
            var result = DSToonNode.FromString(_stringPrimitive);
            return result;
        }

        [Benchmark]
        public DSToonNode ListParseTest()
        {
            var result = DSToonNode.FromString(_stringList);
            return result;
        }

        [Benchmark]
        public DSToonNode DictionaryParseTest()
        {
            var result = DSToonNode.FromString(_stringDictionary);
            return result;
        }
    }
}

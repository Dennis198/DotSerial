using BenchmarkDotNet.Attributes;
using DotSerial.Benchmarks.Helpers;
using DotSerial.Xml;

namespace DotSerial.Benchmarks.Benchmarks.Xml
{
    [MemoryDiagnoser]
    public class XmlParserBenchmarks
    {
        private string _stringPrimitive;
        private string _stringList;
        private string _stringDictionary;

        [GlobalSetup]
        public void Setup()
        {
            var primitiveClass = PrimitiveClass.Create();
            var nodePrimitive = DSXmlNode.ToNode(primitiveClass);
            _stringPrimitive = nodePrimitive.Stringify();

            var listClass = ListClass.Create(50, 50);
            var nodeList = DSXmlNode.ToNode(listClass);
            _stringList = nodeList.Stringify();

            var dicClass = DictionaryClass.Create(50);
            var nodeDictionary = DSXmlNode.ToNode(dicClass);
            _stringDictionary = nodeDictionary.Stringify();
        }

        [Benchmark]
        public DSXmlNode PrimitiveParseTest()
        {
            var result = DSXmlNode.FromString(_stringPrimitive);
            return result;
        }

        [Benchmark]
        public DSXmlNode ListParseTest()
        {
            var result = DSXmlNode.FromString(_stringList);
            return result;
        }

        [Benchmark]
        public DSXmlNode DictionaryParseTest()
        {
            var result = DSXmlNode.FromString(_stringDictionary);
            return result;
        }
    }
}

using BenchmarkDotNet.Attributes;
using DotSerial.Benchmarks.Helpers;
using DotSerial.Xml;

namespace DotSerial.Benchmarks.Benchmarks.Xml
{
    [MemoryDiagnoser]
    public class XmlWriterBenchmarks
    {
        private DSXmlNode _nodePrimitve;
        private DSXmlNode _nodeList;
        private DSXmlNode _nodeDictionary;

        [GlobalSetup]
        public void Setup()
        {
            var primitiveClass = PrimitiveClass.Create();
            _nodePrimitve = DSXmlNode.ToNode(primitiveClass);

            var listClass = ListClass.Create(50, 50);
            _nodeList = DSXmlNode.ToNode(listClass);

            var dicClass = DictionaryClass.Create(50);
            _nodeDictionary = DSXmlNode.ToNode(dicClass);
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

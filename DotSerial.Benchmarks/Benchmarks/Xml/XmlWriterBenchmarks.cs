using BenchmarkDotNet.Attributes;
using DotSerial.Benchmarks.Helpers;
using DotSerial.Xml;

namespace DotSerial.Benchmarks.Benchmarks.Xml
{
    [MemoryDiagnoser]
    public class XmlWriterBenchmarks
    {
        private DSXmlNode? _nodePrimitve;
        private DSXmlNode? _nodeList;
        private DSXmlNode? _nodeDictionary;

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

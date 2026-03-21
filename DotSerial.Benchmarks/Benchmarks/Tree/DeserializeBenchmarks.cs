using BenchmarkDotNet.Attributes;
using DotSerial.Benchmarks.Helpers;
using DotSerial.Tree.Deserialize;
using DotSerial.Tree.Nodes;
using DotSerial.Tree.Serialize;

namespace DotSerial.Benchmarks.Benchmarks.Tree
{
    [MemoryDiagnoser]
    public class DeserializeBenchmarks
    {
        // TODO FÜR ALLE MACHEN YAML; XML; JSPN; TOON
        private IDSNode? _primitiveNode;
        private IDSNode? _listNode;
        private IDSNode? _dictionaryNode;

        [GlobalSetup]
        public void Setup()
        {
            var primitiveClass = PrimitiveClass.Create();
            _primitiveNode = SerializeObject.Serialize(primitiveClass, "0", StategyType.Json);

            var listClass = ListClass.Create(2, 50);
            _listNode = SerializeObject.Serialize(listClass, "0", StategyType.Json);

            var dictionaryClass = DictionaryClass.Create(50);
            _dictionaryNode = SerializeObject.Serialize(dictionaryClass, "0", StategyType.Json);
        }

        [Benchmark]
        public object? PrimitiveDeserialize()
        {
            if (null == _primitiveNode)
            {
                throw new InvalidOperationException("Node is null");
            }
            var result = _primitiveNode.DeserializeAccept(new DeserializeObject(), typeof(PrimitiveClass));
            return result;
        }

        [Benchmark]
        public object? ListDeserialize()
        {
            if (null == _listNode)
            {
                throw new InvalidOperationException("Node is null");
            }
            var result = _listNode.DeserializeAccept(new DeserializeObject(), typeof(ListClass));
            return result;
        }

        [Benchmark]
        public object? DictionaryDeserialize()
        {
            if (null == _dictionaryNode)
            {
                throw new InvalidOperationException("Node is null");
            }
            var result = _dictionaryNode.DeserializeAccept(new DeserializeObject(), typeof(DictionaryClass));
            return result;
        }
    }
}

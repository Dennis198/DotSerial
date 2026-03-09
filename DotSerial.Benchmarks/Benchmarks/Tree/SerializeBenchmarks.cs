using BenchmarkDotNet.Attributes;
using DotSerial.Benchmarks.Helpers;
using DotSerial.Tree.Nodes;
using DotSerial.Tree.Serialize;
using DotSerial.Tree.Creation;

namespace DotSerial.Benchmarks.Benchmarks.Tree
{
    [MemoryDiagnoser]
    public class SerializeBenchmarks
    {
        // TODO FÜR ALLE MACHEN YAML; XML; JSPN; TOON
        private PrimitiveClass? _primitiveClass;
        private ListClass? _listClass;
        private DictionaryClass? _dictionaryClass;

        [GlobalSetup]
        public void Setup()
        {
            _primitiveClass = PrimitiveClass.Create();

            _listClass = ListClass.Create(2, 50);

            _dictionaryClass = DictionaryClass.Create(50);
        }

        [Benchmark]
        public IDSNode PrimitiveSerialize()
        {
            var result = SerializeObject.Serialize(_primitiveClass, "0", StategyType.Json);
            return result;
        }

        [Benchmark]
        public IDSNode ListSerialize()
        {
            var result = SerializeObject.Serialize(_listClass, "0", StategyType.Json);
            return result;
        }

        [Benchmark]
        public IDSNode DictionarySerialize()
        {
            var result = SerializeObject.Serialize(_dictionaryClass, "0", StategyType.Json);
            return result;
        }
    }
}

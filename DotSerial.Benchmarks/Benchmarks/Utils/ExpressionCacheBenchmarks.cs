using System.Reflection;
using BenchmarkDotNet.Attributes;
using DotSerial.Benchmarks.Helpers;

namespace DotSerial.Benchmarks.Benchmarks.Utils
{
    [MemoryDiagnoser]
    public class ExpressionCacheBenchmarks
    {
        private PrimitiveClass _instance = null!;
        private PropertyInfo _propInfo = null!;
        private MethodInfo _enqueueMethod = null!;
        private Queue<int> _queue = null!;

        private Func<object, object> _cachedGetter = null!;
        private Action<object?, object?> _cachedSetter = null!;
        private Action<object?, object?> _cachedMethodInvoker = null!;

        private const string PropName = nameof(PrimitiveClass.Int);
        private const string MethodName = "Enqueue";

        [GlobalSetup]
        public void Setup()
        {
            _instance = PrimitiveClass.Create();
            _propInfo = typeof(PrimitiveClass).GetProperty(PropName)!;
            _queue = new Queue<int>();
            _enqueueMethod = typeof(Queue<int>).GetMethod(MethodName)!;

            _cachedGetter = ExpressionCache.GetOrCreateGetter(typeof(PrimitiveClass), PropName);
            _cachedSetter = ExpressionCache.GetOrCreateSetter(typeof(PrimitiveClass), PropName);
            _cachedMethodInvoker = ExpressionCache.GetOrCreateMethodInvoker(typeof(Queue<int>), MethodName);
        }

        // ── Getter ───────────────────────────────────────────────────────────

        [Benchmark]
        public object? Getter_Reflection() => _propInfo.GetValue(_instance);

        // Measures dict-lookup + key allocation + delegate call (current pattern in SerializeObject)
        [Benchmark]
        public object? Getter_ExpressionCache_WithLookup() =>
            ExpressionCache.GetOrCreateGetter(typeof(PrimitiveClass), PropName)(_instance);

        // Measures only the compiled delegate call (optimal usage)
        [Benchmark]
        public object? Getter_ExpressionCache_Direct() => _cachedGetter(_instance);

        // ── Setter ───────────────────────────────────────────────────────────

        [Benchmark]
        public void Setter_Reflection() => _propInfo.SetValue(_instance, 99);

        // Measures dict-lookup + key allocation + delegate call (current pattern in DeserializeObject)
        [Benchmark]
        public void Setter_ExpressionCache_WithLookup() =>
            ExpressionCache.GetOrCreateSetter(typeof(PrimitiveClass), PropName)(_instance, 99);

        // Measures only the compiled delegate call (optimal usage)
        [Benchmark]
        public void Setter_ExpressionCache_Direct() => _cachedSetter(_instance, 99);

        // ── MethodInvoker ─────────────────────────────────────────────────────

        // GetMethod + Invoke every call (original pattern before caching)
        [Benchmark]
        public void MethodInvoker_GetMethodAndInvoke() => typeof(Queue<int>).GetMethod(MethodName)!.Invoke(_queue, [1]);

        // Cached MethodInfo, still uses reflection Invoke (the old pattern in ConverterMethods)
        [Benchmark]
        public void MethodInvoker_CachedMethodInfo() => _enqueueMethod.Invoke(_queue, [1]);

        // Measures dict-lookup + key allocation + delegate call (current pattern in ConverterMethods)
        [Benchmark]
        public void MethodInvoker_ExpressionCache_WithLookup() =>
            ExpressionCache.GetOrCreateMethodInvoker(typeof(Queue<int>), MethodName)(_queue, 1);

        // Measures only the compiled delegate call (optimal usage)
        [Benchmark]
        public void MethodInvoker_ExpressionCache_Direct() => _cachedMethodInvoker(_queue, 1);
    }
}

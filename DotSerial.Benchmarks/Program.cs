using BenchmarkDotNet.Running;

namespace DotSerial.Benchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = BenchmarkConfig.Create();
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, config);

            // Use for debugging
            //BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new DebugInProcessConfig());
        }
    }
}

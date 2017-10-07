using System;

namespace WhyUAllocate
{
    class Program
    {
        static void Main(string[] args)
        {
           // var config = BenchmarkDotNet.Configs.DefaultConfig.Instance;
           
            BenchmarkDotNet.Running.BenchmarkRunner.Run<TestClass>();
            Console.WriteLine("Hello World!");
        }
    }
}

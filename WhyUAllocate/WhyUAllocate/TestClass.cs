using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace WhyUAllocate
{
    [MemoryDiagnoser]
    public class TestClass
    {

        [Benchmark]
        public int Closure()
        {
            var result = 0;
            for (int i = 0; i < 10000; i++)
            {
                var c = ContainsClosure(i, false);
                if (c != null)
                {
                    result += c();
                }
            }
            return result;
        }

        [Benchmark]
        public int NotClosure()
        {
            var result = 0;
            for(int i = 0; i < 10000; i++)
            {
                var c = ContainsClosure(i, true);
                if(c != null)
                {
                    result += c();
                }
            }
            return result;
        }

        public static Func<int> ContainsClosure(int number, bool skipClosure)
        {
            if(skipClosure)
            {
                return null;
            }

            Func<int> closure = () => number * number;
            return closure;
        }
    }
}

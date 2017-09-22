using System;
using Vacuum.Core;

namespace Vacuum.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            var reader = new PEFileReader(@"C:\code\combined\MergeTest.dll");
            reader.Parse();

            Console.WriteLine("Hello World!");
        }
    }
}

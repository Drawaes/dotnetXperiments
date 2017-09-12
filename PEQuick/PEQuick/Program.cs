using System;

namespace PEQuick
{
    class Program
    {
        static void Main(string[] args)
        {
            PEFile.Load("C:\\code\\combined\\MergeTest.dll");
            Console.WriteLine("Hello World!");
        }
    }
}

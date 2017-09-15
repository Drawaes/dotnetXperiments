using System;
using System.Collections.Generic;

namespace PEQuick
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileList = new string[] { "System.Console.dll" }; // ,
            var files = new Dictionary<string, PEFile>(StringComparer.OrdinalIgnoreCase);

            foreach (var fl in fileList)
            {
                files.Add(fl, PEFile.Load("C:\\code\\combined\\"+ fl));
            }
            Console.WriteLine("Hello World!");
        }
    }
}

using System;
using System.Collections.Generic;

namespace PEQuick
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileList = new string[] {"mergetest.dll", "System.Console.dll"};
            var files = new List< PEFile>();

            foreach (var fl in fileList)
            {
                files.Add(PEFile.Load("C:\\code\\combined\\"+ fl));
            }
            Console.WriteLine("Starting import");

            var import = new Importer.AssemblyImporter(files[0], files[1]);
            import.FindImportPoints();

        }
    }
}

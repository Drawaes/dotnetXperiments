using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace Crusher2
{
    class Program
    {
        static void Main(string[] args)
        {
            string entryModule = @"C:\tests\MergeTest\MergeTest\bin\Debug\netcoreapp2.0\win10-x64\publish\MergeTest.dll";
            string folder = @"C:\tests\MergeTest\MergeTest\bin\Debug\netcoreapp2.0\win10-x64\publish";

            Console.WriteLine("Hello World!");
            var mainModule = ModuleDefinition.ReadModule(entryModule, new ReaderParameters
            {
                AssemblyResolver = new AssemblyResolver(folder)
            });

            var builder = new Strike2.ModuleRebuilder("NewModule");
            var entry =(MethodDefinition) builder.Map(mainModule.EntryPoint, null);

            
            int count;
            do
            {
                count = builder.TypeDefinitions.CleanDirtyTypes(10);

                Console.WriteLine($"Remaining dirty types {count}");
            } while (count > 0);

            builder.Module.Module.EntryPoint = entry;

            //builder.MergeTypes();

            builder.Module.Write("C:\\code\\new.dll");

            //foreach(var t in builder.Module.Module.MetadataSystem.)

            Console.ReadLine();
        }
    }
}

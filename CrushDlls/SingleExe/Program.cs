using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace SingleExe
{
    class Program
    {
        static void Main(string[] args)
        {
            
            
            string moduleName = "Combined";

            var module = ModuleDefinition.ReadModule(entryModule, new ReaderParameters
            {
                AssemblyResolver = new AssemblyResolver(folder)
            });

            FutureModule.Init(moduleName, module.Kind);

            var topType = FutureModule.ResolveMethodDefinition(module.EntryPoint);

            Console.WriteLine($"Remaining work {FutureModule.Iterate(10000)}");

            FutureModule.Save(@"C:\code\output.dll");

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using ILSmasher;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace SingleExe
{
    class Program
    {


        static void Main(string[] args)
        {
            string folder = @"C:\code\netperf\SocketPerfTest\bin\Release\netcoreapp2.1\win10-x64\publish";
            string entryModule = @"C:\code\netperf\SocketPerfTest\bin\Release\netcoreapp2.1\win10-x64\publish\SocketPerfTest.dll";
            string moduleName = "Combined";

            ModuleLoader.Load(folder, entryModule, moduleName);

            ModuleLoader.LoadAllTypes();

            //TypeMapper.PrintTypeList(@"C:\code\types.txt");

            CodeSweeper.AddEntryMethod();
            CodeSweeper.WalkItems(1000);

            TypeMapper.BuildTypes();

            ModuleLoader.Save(@"C:\code\output.dll");
            //var modules = new ILCrusher.ModulesState(folder, entryModule, moduleName);

            //modules.ResolveEntryPoint();

            //modules.FinishWork();

            //modules.Write(@"C:\code\output.dll");
        }
    }
}

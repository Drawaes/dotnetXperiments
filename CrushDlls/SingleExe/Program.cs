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
            var state = new MergeState("Combined");
            state.LoadTypesToMerge(@"C:\code\netperf\SocketPerfTest\bin\Debug\netcoreapp2.1\publish", "*.dll");
            state.GenerateTypeShells();
            state.GenerateTypeHeaders();
            state.ProcessMethods();

            state.Save("C:\\code\\newModule.dll");
            Console.WriteLine("Hello World!");
                                  
            //    if (m.HasBody)
            //    {
            //        

            


            //}

            

        }
    }
}

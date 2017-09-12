using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;

namespace ILSmasher
{
    //public static class ModuleLoader
    //{
    //    static List<ModuleDefinition> _moduleDefinitions = new List<ModuleDefinition>();
    //    public static ModuleDefinition EntryModule;
    //    public static ModuleDefinition OldEntryModule;

    //    static Queue<object> _remainingWork = new Queue<object>();

    //    public static void Load(string pathToModules, string entryModule, string moduleName)
    //    {
    //        OldEntryModule = ModuleDefinition.ReadModule(entryModule, new ReaderParameters(ReadingMode.Immediate)
    //        {
    //            AssemblyResolver = new AssemblyResolver(pathToModules)
    //        });
    //        EntryModule = ModuleDefinition.CreateModule(moduleName, OldEntryModule.Kind);
    //    }

    //    public static void ProcessEntryPoint()
    //    {
    //        EntryModule.EntryPoint = MethodMapper.GetMethodDefinition(OldEntryModule.EntryPoint);
    //    }
                      
    //    public static void Save(string fileName)
    //    {
    //        EntryModule.Write(fileName);
    //    }
    //}
}

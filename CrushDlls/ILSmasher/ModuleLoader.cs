using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace ILSmasher
{
    public static class ModuleLoader
    {
        static List<ModuleDefinition> _moduleDefinitions = new List<ModuleDefinition>();
        public static ModuleDefinition EntryModule;
        public static ModuleDefinition OldEntryModule;

        public static void Load(string pathToModules, string entryModule, string moduleName)
        {

            foreach (var file in System.IO.Directory.GetFiles(pathToModules, "*.dll"))
            {
                try
                {
                    var mod = ModuleDefinition.ReadModule(file);
                    if (string.Compare(file, entryModule, true) == 0)
                    {
                        OldEntryModule = mod;
                        EntryModule = ModuleDefinition.CreateModule(mod.Name, mod.Kind);
                    }
                    _moduleDefinitions.Add(mod);
                }
                catch
                {
                    Console.WriteLine($"File failed to load {System.IO.Path.GetFileName(file)}");
                }
            }
            
        }

        public static void LoadAllTypes()
        {
            foreach (var mod in _moduleDefinitions)
            {
                foreach (var t in mod.Types)
                {
                    TypeMapper.AddType(t);
                }
            }
        }

        public static void Save(string fileName)
        {
            EntryModule.Write(fileName);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace SingleExe
{
    public class MergeState
    {
        private Dictionary<string, ModuleDefinition> modules = new Dictionary<string, ModuleDefinition>();
        private Dictionary<string, TypeDefinition> types = new Dictionary<string, TypeDefinition>();
        private Dictionary<string, TypeDefinition> newTypes = new Dictionary<string, TypeDefinition>();
        private Dictionary<string, TypeState> typeStates = new Dictionary<string, TypeState>();

        internal void GenerateTypeHeaders()
        {
            foreach (var kv in newTypes)
            {
                var typeState = new TypeState(types[kv.Key], kv.Value, this);
                typeStates.Add(kv.Key, typeState);
                typeState.ProcessFields();
            }
        }

        public void ProcessMethods()
        {
            foreach(var kv in typeStates)
            {
                kv.Value.ProcessMethods();
            }
        }

        private ModuleDefinition newModule;

        public MergeState(string moduleName)
        {
            newModule = ModuleDefinition.CreateModule(moduleName, ModuleKind.Dll);
        }

        public int LoadTypesToMerge(string pathToDlls, string searchPattern)
        {
            foreach (var dll in System.IO.Directory.GetFiles(pathToDlls, searchPattern))
            {
                var module = ModuleDefinition.ReadModule(dll);

                foreach (var t in module.Types.ModuleFilter())
                {
                    types.Add(t.SafeName(), t);
                }
            }
            return types.Count;
        }

        public void GenerateTypeShells()
        {
            foreach (var kv in types)
            {
                var newType = new TypeDefinition(kv.Value.Namespace == "" ? kv.Value.Module.Name : kv.Value.Namespace, kv.Value.Name, kv.Value.Attributes);
                newModule.Types.Add(newType);
                newTypes.Add(newType.SafeName(), newType);
            }
        }

        public void Save(string fileName)
        {
            newModule.Write(fileName);
        }

        internal TypeReference GetTypeRef(TypeReference oldTypeRef)
        {
            var oldName = oldTypeRef.SafeName();
            if (!newTypes.TryGetValue(oldName, out TypeDefinition value))
            {
                if (types.ContainsKey(oldName))
                {
                    throw new InvalidProgramException();
                }
                var refvalue = newModule.ImportReference(oldTypeRef);
                return refvalue;
                //typeRefs.Add(oldName, value);
            }
            return value;
        }

        internal TypeReference GetPlaceHolderTypeRef()
        {
            return newTypes.Values.First();
        }
    }
}

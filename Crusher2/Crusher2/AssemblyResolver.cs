using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace Crusher2
{
    public class AssemblyResolver : IAssemblyResolver
    {
        private string _assemblyPath;
        private Dictionary<string, AssemblyDefinition> _cached = new Dictionary<string, AssemblyDefinition>();

        public AssemblyResolver(string path)
        {
            _assemblyPath = path;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public AssemblyDefinition Resolve(AssemblyNameReference name)
        {
            if (_cached.TryGetValue(name.Name, out AssemblyDefinition def))
            {
                return def;
            }
            def = ModuleDefinition.ReadModule(System.IO.Path.Combine(_assemblyPath, name.Name + ".dll"), new ReaderParameters()
            {
                AssemblyResolver = this
            }).Assembly;
            _cached.Add(name.Name, def);
            return def;
        }

        public AssemblyDefinition Resolve(AssemblyNameReference name, ReaderParameters parameters)
        {
            throw new NotImplementedException();
        }
    }
}

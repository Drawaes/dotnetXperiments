using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Crusher2.Strike2
{
    public class TypeDefinitionHandler
    {
        private ModuleRebuilder _builder;
        private Dictionary<Key, TypeGenerator> _typeGenerators = new Dictionary<Key, TypeGenerator>();
        private Queue<TypeGenerator> _dirtyGenerators = new Queue<TypeGenerator>();

        public TypeDefinitionHandler(ModuleRebuilder builder) => _builder = builder;

        public TypeGenerator Map(TypeDefinition oldDef)
        {
            Debug.Assert(oldDef.Module.Name != _builder.Module.Module.Name);

            //First get the top level type that has the definition

            var topLevel = GetModuleLevelDefinition(oldDef);
            if(_typeGenerators.TryGetValue(topLevel.GetKey(), out TypeGenerator value))
            {
                return value.GetType(oldDef);
            }

            var match = _typeGenerators.Values.SingleOrDefault(d => d.Definition.GetStringKey() == topLevel.GetStringKey());

            if(match != null)
            {
                _typeGenerators.Add(topLevel.GetKey(), match);
                match.MergeType(topLevel);
                return match.GetType(oldDef);
            }

            var generator = new TypeGenerator(topLevel, _builder);
            _typeGenerators.Add(topLevel.GetKey(), generator);
            _builder.AddTypeToModule(generator.Definition);
            _dirtyGenerators.Enqueue(generator);
            generator.WriteGenericParameters(null);
            
            return generator.GetType(oldDef);
        }

        private TypeDefinition GetModuleLevelDefinition(TypeDefinition currentDef)
        {
            if (currentDef.DeclaringType == null)
            {
                return currentDef;
            }
            return GetModuleLevelDefinition(currentDef.DeclaringType);
        }

        internal void MergeTypes()
        {
            var groups = _typeGenerators.GroupBy(kv => kv.Value.Definition.FullName, kv => kv.Value);
            foreach (var g in groups)
            {
                if (g.Count() == 1)
                {
                    continue;
                }
                //foreach (var td in g.Distinct().Skip(1))
                //{
                //    _builder.Module.Module.Types.Remove(td.Definition);
                //}
            }
        }

        internal void MarkDirty(TypeGenerator typeGenerator)
        {
            _dirtyGenerators.Enqueue(typeGenerator);
        }

        internal int CleanDirtyTypes(int maxIterations)
        {
            for(var i = 0; i < maxIterations && _dirtyGenerators.Count > 0; i++)
            {
                var item = _dirtyGenerators.Dequeue();
                item.Clean();
            }
            return _dirtyGenerators.Count;
        }
    }
}

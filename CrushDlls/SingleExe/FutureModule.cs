using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;
using Mono.Collections.Generic;

namespace SingleExe
{
    public static class FutureModule
    {
        private static Dictionary<Key, TypeContainer> _typeContainer = new Dictionary<Key, TypeContainer>();
        private static ModuleDefinition _module;
        private static Queue<object> _dirtyTypes = new Queue<object>();


        public static void Init(string moduleName, ModuleKind kind) => _module = ModuleDefinition.CreateModule(moduleName, kind);

        public static void AddDirtyType(object typeContainer) => _dirtyTypes.Enqueue(typeContainer);

        public static TypeDefinition ResolveTypeDefinition(TypeDefinition type) => ResolveTypeContainer(type).Definition;

        public static TypeContainer ResolveTypeContainer(TypeDefinition type)
        {
            var container = GetOrCreateTopType(FindTopType(type));
            container = container.ResolveType(type);
            return container;
        }

        public static TypeReference Import(TypeReference type)
        {
            return _module.ImportReference(type);
        }

        public static MethodDefinition ResolveMethodDefinition(MethodDefinition method)
        {
            if(method.IsGenericInstance)
            {
                throw new NotImplementedException();
            }
            var hostContainer = ResolveTypeContainer(method.DeclaringType);
            return hostContainer.ResolveMethodDefinition(method);
        }

        private static TypeContainer GetOrCreateTopType(TypeDefinition type)
        {
            var key = type.GetKey();
            if (_typeContainer.TryGetValue(key, out TypeContainer container))
            {

                return container;
            }
            var newContainer = new TypeContainer(type);
            _typeContainer.Add(key, newContainer);
            newContainer.Init();
            AddDirtyType(newContainer);
            _module.Types.Add(newContainer.Definition);
            return newContainer;
        }

        internal static FieldDefinition ResolveFieldDefinition(FieldDefinition fieldDef)
        {
            var typeContainer = ResolveTypeContainer(fieldDef.DeclaringType);
            return typeContainer.ResolveFieldDefinition(fieldDef);
        }

        private static TypeDefinition FindTopType(TypeDefinition typeDefinition)
        {
            if(typeDefinition.IsGenericInstance)
            {
                throw new InvalidCastException();
            }
            if (typeDefinition.DeclaringType != null)
            {
                return FindTopType(typeDefinition.DeclaringType);
            }
            return typeDefinition;
        }

        public static MethodReference ResolveMethodReference(MethodReference reference)
        {
            switch(reference)
            {
                case MethodDefinition methodDef:
                    return ResolveMethodDefinition(methodDef);
            }
        }

        public static void Save(string fileName) => _module.Write(fileName);

        public static int Iterate(int maxCount)
        {
            for (int i = 0; i < maxCount && _dirtyTypes.Count > 0; i++)
            {
                var item = _dirtyTypes.Dequeue();
                switch (item)
                {
                    case MethodContainer methodContainer:
                        methodContainer.Finish();
                        break;
                    case TypeContainer typeContainer:
                        typeContainer.Finish();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            return _dirtyTypes.Count;
        }
    }
}

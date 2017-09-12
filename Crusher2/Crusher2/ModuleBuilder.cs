using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Crusher2
{
    public class ModuleBuilder
    {
        private ModuleDefinition _module;
        private Queue<TypeDefinition> _waitingForFields = new Queue<TypeDefinition>();
        private Dictionary<Key, TypeDefinition> _oldKeyToNewKey = new Dictionary<Key, TypeDefinition>();
        private Dictionary<Key, GenericParameter> _oldGenParamToNewParam = new Dictionary<Key, GenericParameter>();

        public ModuleBuilder(string name, ModuleKind moduleKind)
        {
            _module = ModuleDefinition.CreateModule(name, moduleKind);
        }
                
        public void CreateTopLevelTypes(Dictionary<Key, object> objectList)
        {
            Console.WriteLine($"Object types first pass {objectList.Count} objects still to process");
            var distinctList = objectList
                .Where(kv => kv.Key.Token.TokenType == TokenType.TypeDef)
                .Select(kv => kv.Value)
                .Distinct()
                .Cast<TypeDefinition>()
                .Where(bt => bt.BaseType == null && bt.DeclaringType == null)
                .ToList();

            foreach (var t in distinctList)
            {
                WriteTypeShell(t);
            }
            foreach (var t in distinctList)
            {
                var fullList = objectList.Where(kv => kv.Key.Token.TokenType == TokenType.TypeDef
                && ((TypeDefinition)kv.Value).GetKey() == t.GetKey()).ToList();
                foreach (var kv in fullList)
                {
                    objectList.Remove(kv.Key);
                    _oldKeyToNewKey[kv.Key] = _oldKeyToNewKey[((TypeDefinition)kv.Value).GetKey()];
                }
            }
            Console.WriteLine($"First pass completed {objectList.Count} objects still to process");
        }
                
        private void WriteTypeShell(TypeDefinition typeDef)
        {
            var newType = new TypeDefinition(typeDef.GetSafeNamespace(), typeDef.Name, typeDef.Attributes)
            {
                BaseType = MapTypeReference(typeDef.BaseType),
                ClassSize = typeDef.ClassSize,
                DeclaringType = MapTypeDefinition(typeDef.DeclaringType),
                IsAbstract = typeDef.IsAbstract,
                IsAnsiClass = typeDef.IsAnsiClass,
                IsAutoClass = typeDef.IsAutoClass,
                IsAutoLayout = typeDef.IsAutoLayout,
                IsBeforeFieldInit = typeDef.IsBeforeFieldInit,
                IsExplicitLayout = typeDef.IsExplicitLayout,
                IsInterface = typeDef.IsInterface,
                IsPublic = typeDef.IsPublic,
                IsRuntimeSpecialName = typeDef.IsRuntimeSpecialName,
                IsSealed = typeDef.IsSealed,
                IsSequentialLayout = typeDef.IsSequentialLayout,
                IsSerializable = typeDef.IsSerializable,
                IsSpecialName = typeDef.IsSpecialName,
                PackingSize = typeDef.PackingSize
            };
            if (newType.DeclaringType == null)
            {
                _module.Types.Add(newType);
            }
            else
            {
                newType.DeclaringType.NestedTypes.Add(newType);
            }
            _oldKeyToNewKey.Add(typeDef.GetKey(), newType);
            _waitingForFields.Enqueue(typeDef);
        }

        private TypeDefinition MapTypeDefinition(TypeDefinition typeDef)
        {
            if (typeDef == null)
            {
                return null;
            }
            if (_oldKeyToNewKey.TryGetValue(typeDef.GetKey(), out TypeDefinition value))
            {
                return value;
            }
            throw new NotImplementedException();
        }

        private TypeReference MapTypeReference(TypeReference typeRef)
        {
            if (typeRef == null)
            {
                return null;
            }
            if (typeRef.IsGenericInstance)
            {
                throw new NotImplementedException();
            }

            var typeDef = typeRef.Resolve();
            if (typeDef == null)
            {
                throw new NotImplementedException();
            }
            return MapTypeDefinition(typeDef);
        }
    }
}

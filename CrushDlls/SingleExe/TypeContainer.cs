using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace SingleExe
{
    public class TypeContainer
    {
        private TypeDefinition _definition;
        private TypeDefinition _oldDefinition;
        private Dictionary<Key, TypeContainer> _nestedTypes = new Dictionary<Key, TypeContainer>();
        private Dictionary<Key, MethodContainer> _methods = new Dictionary<Key, MethodContainer>();
        private Key _key;

        public TypeContainer(TypeDefinition type)
        {
            _key = type.GetKey();
            _oldDefinition = type;
            _definition = new TypeDefinition(type.GetSafeNamespace(), type.Name, type.Attributes);
        }

        public void Init()
        {
            foreach (var p in _oldDefinition.GenericParameters)
            {
                var param = new GenericParameter(p.Name, _definition)
                {
                    Attributes = p.Attributes
                };
                _definition.GenericParameters.Add(param);
            }
            //TODO Contraints and CustomAttributes on Generic Params
            //TODO CustomAttributes on the class
            foreach(var method in _oldDefinition.Methods)
            {
                if(method.IsStatic && method.IsConstructor)
                {
                    ResolveMethodDefinition(method);
                }
            }
            if(_oldDefinition.BaseType != null)
            {
                _definition.BaseType = TypeReferences.ResolveTypeReference(_oldDefinition.BaseType, _definition);
            }
        }

        public TypeDefinition Definition => _definition;
        public Key Key => _key;

        internal TypeContainer ResolveType(TypeDefinition type)
        {
            if(_key == type)
            {
                return this;
            }

            var topType = FindFirstChild(type);

            if (_nestedTypes.TryGetValue(topType.GetKey(), out TypeContainer container))
            {
                return container.ResolveType(type);
            }
            var newContainer = new TypeContainer(topType);
            _nestedTypes.Add(newContainer.Key, newContainer);
            newContainer.Init();
            FutureModule.AddDirtyType(newContainer);
            _definition.NestedTypes.Add(newContainer.Definition);
            return newContainer.ResolveType(type);
        }

        public MethodDefinition ResolveMethodDefinition(MethodDefinition method)
        {
            if(_methods.TryGetValue(method.GetKey(), out MethodContainer value))
            {
                return value.Definition;
            }

            var newMethod = new MethodContainer(method);
            _methods.Add(newMethod.Key, newMethod);
            newMethod.Init(_definition);
            FutureModule.AddDirtyType(newMethod);
            _definition.Methods.Add(newMethod.Definition);
            return newMethod.Definition;
        }

        public FieldDefinition ResolveFieldDefinition(FieldDefinition fieldDef)
        {
            var returnField = _definition.Fields.SingleOrDefault(f => f.Name == fieldDef.Name);
            if(returnField != null)
            {
                return returnField;
            }
            fieldDef.DeclaringType = _definition;
            fieldDef.FieldType = TypeReferences.ResolveTypeReference(fieldDef.FieldType, _definition);
            fieldDef.CustomAttributes.Clear();
            //TODO CustomAttributes
            _definition.Fields.Add(fieldDef);
            return returnField;
        }

        private TypeDefinition FindFirstChild(TypeDefinition typeDef)
        {
            if (typeDef.DeclaringType == null)
            {
                throw new InvalidOperationException("Can't find a matching top type in the tree");
            }
            if(_key == typeDef.DeclaringType)
            {
                return typeDef;
            }
            return FindFirstChild(typeDef.DeclaringType);
        }

        internal void Finish()
        {
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Crusher2.Strike2
{
    public class TypeGenerator
    {
        private TypeDefinition _newDefinition;
        private TypeDefinition _oldDefinition;
        private ModuleRebuilder _builder;
        private Dictionary<Key, MethodGenerator> _methods = new Dictionary<Key, MethodGenerator>();
        private Queue<MethodGenerator> _incompleteMethods = new Queue<MethodGenerator>();
        private Dictionary<Key, TypeGenerator> _nestedTypes = new Dictionary<Key, TypeGenerator>();
        private bool _finishedInit;

        public TypeGenerator(TypeDefinition oldDef, ModuleRebuilder builder)
        {
            _oldDefinition = oldDef;
            _builder = builder;

            _newDefinition = new TypeDefinition(_oldDefinition.GetSafeNamespace(), _oldDefinition.Name, _oldDefinition.Attributes);
            if (_oldDefinition.HasLayoutInfo)
            {
                _newDefinition.ClassSize = _oldDefinition.ClassSize;
                _newDefinition.PackingSize = _oldDefinition.PackingSize;
            };
        }

        public ModuleRebuilder Builder => _builder;
        public TypeDefinition Definition => _newDefinition;

        internal void WriteGenericParameters(IGenericParameterProvider parmProvider)
        {
            parmProvider = parmProvider ?? _newDefinition;

            _builder.Map(_oldDefinition.GenericParameters, _newDefinition.GenericParameters, parmProvider);
        }

        internal void MergeType(TypeDefinition topLevel)
        {
            foreach (var m in topLevel.Methods)
            {
                GetMethod(m);
            }

            foreach (var f in topLevel.Fields)
            {
                GetField(f);
            }
        }

        internal TypeGenerator GetType(TypeDefinition typeDefinition)
        {
            if (typeDefinition.GetStringKey() == _oldDefinition.GetStringKey())
            {
                return this;
            }
            var topLevelType = GetTopLevelType(typeDefinition);
            if (_nestedTypes.TryGetValue(topLevelType.GetKey(), out TypeGenerator value))
            {
                return value.GetType(typeDefinition);
            }

            var match = _nestedTypes.Values.SingleOrDefault(d => d.Definition.GetStringKey() == topLevelType.GetStringKey());

            if (match != null)
            {
                _nestedTypes.Add(topLevelType.GetKey(), match);
                match.MergeType(topLevelType);
                return match.GetType(typeDefinition);
            }

            //Need to generate the nested type
            var generator = new TypeGenerator(topLevelType, _builder);
            _nestedTypes.Add(topLevelType.GetKey(), generator);
            _newDefinition.NestedTypes.Add(generator.Definition);
            Builder.TypeDefinitions.MarkDirty(generator);
            generator.WriteGenericParameters(null);
            return generator.GetType(typeDefinition);
        }

        private TypeDefinition GetTopLevelType(TypeDefinition typeDef)
        {
            if (typeDef.DeclaringType.GetKey() == _oldDefinition.GetKey())
            {
                return typeDef;
            }
            return GetTopLevelType(typeDef.DeclaringType);
        }

        internal MethodDefinition GetMethod(MethodDefinition oldMethod)
        {
            if (oldMethod.DeclaringType.GetStringKey() != _oldDefinition.GetStringKey())
            {
                return GetType(oldMethod.DeclaringType).GetMethod(oldMethod);
            }

            if (_methods.TryGetValue(oldMethod.GetKey(), out MethodGenerator value))
            {
                return value.Definition;
            }

            value = _methods.Values.SingleOrDefault(m => _builder.MethodComparer.Equals(m.Definition, oldMethod));
            if (value != null)
            {
                return value.Definition;
            }

            var methodGen = new MethodGenerator(oldMethod, this);
            _methods.Add(oldMethod.GetKey(), methodGen);
            _incompleteMethods.Enqueue(methodGen);
            _builder.TypeDefinitions.MarkDirty(this);
            methodGen.FinishArguments();
            return methodGen.Definition;
        }

        internal void Clean()
        {
            FinishInit();

            while (_incompleteMethods.Count > 0)
            {
                var item = _incompleteMethods.Dequeue();
                item.Finish();
            }
        }

        private void FinishInit()
        {
            if (_finishedInit)
            {
                return;
            }
            _finishedInit = true;
            if (_oldDefinition.BaseType != null && _newDefinition.BaseType == null)
            {
                _newDefinition.BaseType = _builder.Map(_oldDefinition.BaseType, _newDefinition);
            }

            //Add static constructor
            foreach (var m in _oldDefinition.Methods)
            {
                GetMethod(m);
            }

            _builder.Map(_oldDefinition.CustomAttributes, _newDefinition.CustomAttributes, _newDefinition);

            foreach (var field in _oldDefinition.Fields)
            {
                if (!_newDefinition.Fields.Any(f => f.Name == field.Name))
                {
                    GetField(field);
                }
            }

            foreach (var i in _oldDefinition.Interfaces)
            {
                if (!_newDefinition.Interfaces.Any(i2 => i.InterfaceType.FullName == i2.InterfaceType.FullName))
                {
                    _newDefinition.Interfaces.Add(new InterfaceImplementation(_builder.Map(i.InterfaceType, _newDefinition)));
                }
            }

            foreach (var p in _oldDefinition.Properties)
            {
                var newProp = new PropertyDefinition(p.Name, p.Attributes, _builder.Map(p.PropertyType, _newDefinition));
                _newDefinition.Properties.Add(newProp);
                if (p.SetMethod != null)
                {
                    newProp.SetMethod = GetMethod(p.SetMethod);
                }

                if (p.GetMethod != null)
                {
                    newProp.GetMethod = GetMethod(p.GetMethod);
                }
                if (p.HasOtherMethods)
                {
                    foreach (var meth in p.OtherMethods)
                    {
                        var nm = GetMethod(meth);
                        if (nm != null)
                        {
                            newProp.OtherMethods.Add(nm);
                        }
                    }
                }
            }
        }

        internal FieldDefinition GetField(FieldDefinition fieldDef)
        {
            if (fieldDef.DeclaringType.GetStringKey() != _oldDefinition.GetStringKey())
            {
                var decGen = GetType(fieldDef.DeclaringType);
                return decGen.GetField(fieldDef);
            }
            var field = _newDefinition.Fields.SingleOrDefault(f => f.Name == fieldDef.Name);
            if (field != null)
            {
                return field;
            }
            var newFieldDef = _builder.FieldMapper.Copy(fieldDef, _newDefinition);

            _newDefinition.Fields.Add(newFieldDef);
            return newFieldDef;
        }
    }
}

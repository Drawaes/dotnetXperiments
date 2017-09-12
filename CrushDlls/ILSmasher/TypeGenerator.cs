using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace ILSmasher
{
    //public class TypeGenerator
    //{
    //    private Key _key;
    //    private TypeDefinition _newDefinition;
    //    private TypeDefinition _oldDefinition;

    //    public TypeGenerator(TypeDefinition oldDefinition)
    //    {
    //        _oldDefinition = oldDefinition;
    //        _newDefinition = new TypeDefinition(oldDefinition.GetSafeNamespace(), oldDefinition.Name, oldDefinition.Attributes)
    //        {
    //            ClassSize = _oldDefinition.ClassSize,
    //            PackingSize = _oldDefinition.PackingSize
    //        };
            
    //        _key = oldDefinition.GetKey();
    //    }

    //    public void WireTypeToParent()
    //    {
    //        if (_oldDefinition.DeclaringType == null)
    //        {
    //            ModuleLoader.EntryModule.Types.Add(_newDefinition);
    //        }
    //        else
    //        {
    //            var parentType = TypeMapper.GetTypeDefinition(_oldDefinition.DeclaringType);
    //            _newDefinition.DeclaringType = parentType;
    //        }
    //        foreach (var t in _oldDefinition.NestedTypes)
    //        {
    //            var newType = TypeMapper.GetTypeDefinition(t);
    //            newType.DeclaringType = _newDefinition;
    //            _newDefinition.NestedTypes.Add(newType);
    //        }
    //        GenericMapper.GetGenericParameters(_oldDefinition.GenericParameters, _newDefinition.GenericParameters, _newDefinition);
    //    }

    //    internal void Finish()
    //    {
    //        //foreach(var m in _oldDefinition.Methods)
    //        //{
    //        //    if(m.IsConstructor)
    //        //    {
    //        //        MethodMapper.GetMethodDefinition(m);
    //        //    }
    //        //}
    //        //_newDefinition.IsInterface = _oldDefinition.IsInterface;

    //        if (_oldDefinition.BaseType != null)
    //        {
    //            _newDefinition.BaseType = TypeMapper.GetTypeReference(_oldDefinition.BaseType, _newDefinition);
    //        }

    //        //TypeMapper.GetCustomAttributes(_oldDefinition.CustomAttributes, _newDefinition.CustomAttributes);
    //    }

    //    public TypeDefinition Definition => _newDefinition;
    //}
}

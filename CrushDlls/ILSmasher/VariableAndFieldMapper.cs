using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace ILSmasher
{
    //public static class VariableAndFieldMapper
    //{
    //    public static void GetVariables(Collection<VariableDefinition> old, Collection<VariableDefinition> newList, IGenericParameterProvider parmProvider)
    //    {
    //        foreach(var v in old)
    //        {
    //            newList.Add(GetVariable(v, parmProvider));
    //        }
    //    }

    //    public static void RemapVariables(Collection<VariableDefinition> old, IGenericParameterProvider parmProvider)
    //    {
    //        for(int i = 0;i< old.Count;i++)
    //        {
    //            old[i] = GetVariable(old[i],parmProvider);
    //        }
    //    }
        
    //    public static VariableDefinition GetVariable(VariableDefinition old, IGenericParameterProvider parmProvider)
    //    {
    //        return new VariableDefinition(TypeMapper.GetTypeReference(old.VariableType, parmProvider));
    //    }

    //    public static FieldDefinition GetFieldDefinition(FieldDefinition fieldDef, IGenericParameterProvider parmProvider)
    //    {
    //        var newDef = new FieldDefinition(fieldDef.Name, fieldDef.Attributes, TypeMapper.GetTypeReference(fieldDef.FieldType, parmProvider));
    //        var decType = TypeMapper.GetTypeDefinition(fieldDef.DeclaringType);
    //        decType.Fields.Add(newDef);
    //        newDef.Offset = fieldDef.Offset;
    //        newDef.Constant = fieldDef.Constant;
    //        //TypeMapper.GetCustomAttributes(fieldDef.CustomAttributes, newDef.CustomAttributes);
    //        return newDef;
    //    }

    //    public static FieldReference GetFieldReference(FieldReference fieldRef)
    //    {
    //        if(fieldRef is FieldDefinition fieldDef)
    //        {
    //            return GetFieldDefinition(fieldDef);
    //        }
    //        var resolve = fieldRef.Resolve();
    //        return GetFieldDefinition(resolve);
    //    }

    //    public static FieldDefinition GetFieldDefinition(FieldDefinition fieldDef)
    //    {
    //        var parentType = TypeMapper.GetTypeDefinition(fieldDef.DeclaringType);
    //        var newFieldDef = new FieldDefinition(fieldDef.Name, fieldDef.Attributes, TypeMapper.GetTypeReference(fieldDef.FieldType, parentType))
    //        {
    //            Constant = fieldDef.Constant,
    //            InitialValue = fieldDef.InitialValue,
    //            Offset = fieldDef.Offset,
    //        };
    //        //TypeMapper.GetCustomAttributes(fieldDef.CustomAttributes, newFieldDef.CustomAttributes);
    //        parentType.Fields.Add(newFieldDef);
    //        return newFieldDef;
    //    }
    //}
}

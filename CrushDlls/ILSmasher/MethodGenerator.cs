using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace ILSmasher
{
    //public class MethodGenerator
    //{
    //    private MethodDefinition _oldMethod;
    //    private MethodDefinition _newMethod;

    //    public MethodGenerator(MethodDefinition oldMethod)
    //    {
    //        _oldMethod = oldMethod;
    //        var parentType = TypeMapper.GetTypeDefinition(_oldMethod.DeclaringType);
    //        _newMethod = new MethodDefinition(oldMethod.Name, oldMethod.Attributes, TypeMapper.GetTypeReference(_oldMethod.ReturnType, parentType));
            
    //        foreach (var gen in _oldMethod.GenericParameters)
    //        {
    //            _newMethod.GenericParameters.Add(GenericMapper.GetGenericParameter(gen, _newMethod));
    //        }

    //        TypeMapper.GetParameterDefinitions(_oldMethod.Parameters, _newMethod.Parameters, _newMethod);
    //    }

    //    public void WireUpToParent()
    //    {
    //        var parentType = TypeMapper.GetTypeDefinition(_oldMethod.DeclaringType);
    //        _newMethod.DeclaringType = parentType;
    //        //parentType.Methods.Add(_newMethod);
            
    //    }

    //    public MethodDefinition Definition => _newMethod;

    //    internal void Finish()
    //    {
            
            
    //        WriteBody();
    //    }

    //    public void WriteBody()
    //    {
    //        if (!_oldMethod.HasBody)
    //        {
    //            return;
    //        }
    //        VariableAndFieldMapper.RemapVariables(_oldMethod.Body.Variables, _newMethod);
    //        foreach (var i in _oldMethod.Body.Instructions)
    //        {
    //            switch (i.Operand)
    //            {
    //                case CallSite callSite:
    //                    callSite.ReturnType = TypeMapper.GetTypeReference(callSite.ReturnType, _newMethod);
    //                    break;
    //                case FieldDefinition fieldDef:
    //                    i.Operand = VariableAndFieldMapper.GetFieldDefinition(fieldDef, _newMethod);
    //                    break;
    //                case FieldReference fieldRef:
    //                    i.Operand = VariableAndFieldMapper.GetFieldReference(fieldRef);
    //                    break;
    //                case TypeDefinition typeDef:
    //                    i.Operand = TypeMapper.GetTypeDefinition(typeDef);
    //                    break;
    //                case MethodDefinition methodDef:
    //                    i.Operand = MethodMapper.GetMethodDefinition(methodDef);
    //                    break;
    //                case MethodReference methodRef:
    //                    i.Operand = MethodMapper.GetMethodReference(methodRef, _newMethod);
    //                    break;
    //                case TypeReference typeRef:
    //                    i.Operand = TypeMapper.GetTypeReference(typeRef, _newMethod);
    //                    break;
    //            }
    //        }

    //        foreach(var e in _oldMethod.Body.ExceptionHandlers)
    //        {
    //            if(e.HandlerType != Mono.Cecil.Cil.ExceptionHandlerType.Catch)
    //            {
    //                continue;
    //            }
    //            e.CatchType = TypeMapper.GetTypeReference(e.CatchType, _newMethod);
    //        }

    //        _newMethod.Body = _oldMethod.Body;
    //    }
    //}
}
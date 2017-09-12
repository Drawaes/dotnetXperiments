using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;
using Mono.Collections.Generic;

namespace ILSmasher
{
    //public static class TypeMapper
    //{
    //    private static Dictionary<Key, TypeGenerator> _types = new Dictionary<Key, TypeGenerator>();
    //    private static Queue<TypeGenerator> _backlog = new Queue<TypeGenerator>();

    //    public static int ItemsInBacklog => _backlog.Count;

    //    internal static int Iterate(int count)
    //    {
    //        if (_backlog.Count == 0)
    //        {
    //            return 0;
    //        }

    //        var counter = 0;
    //        do
    //        {
    //            var item = _backlog.Dequeue();
    //            Console.WriteLine($"Finishing type {item.Definition.FullName}");
    //            item.Finish();

    //            counter++;
    //            count--;
    //        } while (count > 0 && _backlog.Count > 0);
    //        return counter;
    //    }

    //    public static TypeDefinition GetTypeDefinition(TypeDefinition type)
    //    {
    //        if (_types.TryGetValue(type.GetKey(), out TypeGenerator gen))
    //        {
    //            return gen.Definition;
    //        }

    //        var typeGen = new TypeGenerator(type);
    //        _types.Add(type.GetKey(), typeGen);
    //        _backlog.Enqueue(typeGen);
    //        typeGen.WireTypeToParent();
    //        return typeGen.Definition;
    //    }

    //    public static ParameterDefinition GetParameterDefinition(ParameterDefinition parmDef, IGenericParameterProvider parmProvider)
    //    {
    //        //We will remap the top type and steal this completely
    //        parmDef.ParameterType = GetTypeReference(parmDef.ParameterType, parmProvider);
    //        //RemapCustomAttributes(parmDef.CustomAttributes);
    //        return parmDef;
    //    }

    //    public static void GetParameterDefinitions(Collection<ParameterDefinition> old, Collection<ParameterDefinition> newList, IGenericParameterProvider parmProvider)
    //    {
    //        foreach (var p in old)
    //        {
    //            newList.Add(GetParameterDefinition(p, parmProvider));
    //        }
    //    }

    //    public static void RemapCustomAttributes(Collection<CustomAttribute> old)
    //    {
    //        for(int i = 0; i < old.Count;i++)
    //        {
    //            old[i] = GetCustomAttribute(old[i]);
    //        }
    //    }

    //    public static TypeReference GetTypeReference(TypeReference typeRef, IGenericParameterProvider parmProvider)
    //    {
    //        switch (typeRef)
    //        {
    //            case ArrayType array:
    //                return new ArrayType(GetTypeReference(array.ElementType, parmProvider));
    //            case TypeDefinition typeDef:
    //                return GetTypeDefinition(typeDef);
    //                
    //            case SentinelType sentinel:
    //                return new SentinelType(GetTypeReference(sentinel.ElementType, parmProvider));
    //            case GenericInstanceType genType:
    //                return GetGenericInstance(genType, parmProvider);
    //            case GenericParameter genParam:
    //                return GenericMapper.GetGenericParameter(genParam, parmProvider);
    //            case RequiredModifierType modType:
    //                return GetRequiredModifierType(modType, parmProvider);
    //            case TypeSpecification typeSpec:
    //                throw new NotImplementedException();

    //            default:
    //                return GetTypeDefinition(typeRef.Resolve());
    //        }
    //    }

    //    public static RequiredModifierType GetRequiredModifierType(RequiredModifierType modType, IGenericParameterProvider parmProvider)
    //    {
    //        var newDef = new RequiredModifierType(GetTypeReference(modType.ModifierType, null), GetTypeReference(modType.ElementType, parmProvider));
    //        return newDef;
    //    }


    //    public static void GetTypeReferences(Collection<TypeReference> old, Collection<TypeReference> newList, IGenericParameterProvider parmProvider)
    //    {
    //        foreach (var t in old)
    //        {
    //            newList.Add(GetTypeReference(t, parmProvider));
    //        }
    //    }

    //    public static void GetCustomAttributeArguments(Collection<CustomAttributeArgument> old, Collection<CustomAttributeArgument> newList)
    //    {
    //        foreach (var c in old)
    //        {
    //            newList.Add(GetCustomAttributeArgument(c));
    //        }
    //    }

    //    public static void GetCustomAttributeNamedArguments(Collection<CustomAttributeNamedArgument> old, Collection<CustomAttributeNamedArgument> newList)
    //    {
    //        foreach (var c in old)
    //        {
    //            var newArg = new CustomAttributeNamedArgument(c.Name, GetCustomAttributeArgument(c.Argument));
    //            newList.Add(newArg);
    //        }
    //    }

    //    public static CustomAttributeArgument GetCustomAttributeArgument(CustomAttributeArgument old) => new CustomAttributeArgument(GetTypeReference(old.Type, null), old.Value);

    //    public static void GetCustomAttributes(Collection<CustomAttribute> old, Collection<CustomAttribute> newList)
    //    {
    //        foreach (var c in old)
    //        {
    //            newList.Add(GetCustomAttribute(c));
    //        }
    //    }

    //    public static CustomAttribute GetCustomAttribute(CustomAttribute oldAttribue)
    //    {
    //        var constructor = MethodMapper.GetMethodReference(oldAttribue.Constructor, null);
    //        var newAttribute = new CustomAttribute(constructor);
            

    //        GetCustomAttributeArguments(oldAttribue.ConstructorArguments, newAttribute.ConstructorArguments);
    //        GetCustomAttributeNamedArguments(oldAttribue.Fields, newAttribute.Fields);
    //        GetCustomAttributeNamedArguments(oldAttribue.Properties, newAttribute.Properties);
    //        return newAttribute;
    //    }


        
    //}
}

using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace ILSmasher
{
    //public static class MethodMapper
    //{
    //    private static Dictionary<Key, MethodGenerator> _methodDefinitions = new Dictionary<Key, MethodGenerator>();
    //    private static Queue<MethodGenerator> _backlog = new Queue<MethodGenerator>();

    //    public static int ItemsInBacklog => _backlog.Count;

    //    public static MethodDefinition GetMethodDefinition(MethodDefinition methodDef)
    //    {
    //        var key = methodDef.GetKey();
    //        if(_methodDefinitions.TryGetValue(key, out MethodGenerator newDef))
    //        {
    //            return newDef.Definition;
    //        }
    //        var generator = new MethodGenerator(methodDef);
    //        _methodDefinitions.Add(key, generator);
    //        generator.WireUpToParent();
    //        _backlog.Enqueue(generator);
    //        return generator.Definition;
    //    }

    //    public static MethodReference GetMethodReference(MethodReference methodReference, IGenericParameterProvider parmProvider)
    //    {
    //        if(methodReference is GenericInstanceMethod genMethod)
    //        {
    //            var newInstance = new GenericInstanceMethod(GetMethodReference(genMethod.ElementMethod, parmProvider));
    //            TypeMapper.GetTypeReferences(genMethod.GenericArguments, newInstance.GenericArguments, newInstance.ElementMethod);
    //            return newInstance;
    //        }
    //        if(methodReference is MethodDefinition methodDef)
    //        {
    //            return GetMethodDefinition(methodDef);
    //        }
    //        var resolve = methodReference.Resolve();
    //        if(resolve != null)
    //        {
    //            return GetMethodDefinition(resolve);
    //        }
    //        //throw new NotImplementedException();
    //        //Need to make a reference
    //        var newRef = new MethodReference(methodReference.Name, TypeMapper.GetTypeReference(methodReference.ReturnType, parmProvider), TypeMapper.GetTypeReference(methodReference.DeclaringType, parmProvider));
    //        TypeMapper.GetParameterDefinitions(methodReference.Parameters, newRef.Parameters, newRef.DeclaringType);
    //        GenericMapper.GetGenericParameters(methodReference.GenericParameters, newRef.GenericParameters, newRef.DeclaringType);
    //        return newRef;
    //    }

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
    //            Console.WriteLine($"Finishing method {item.Definition.FullName}");
    //            item.Finish();

    //            counter++;
    //            count--;
    //        } while (count > 0 && _backlog.Count > 0);
    //        return counter;
    //    }
    //}
}

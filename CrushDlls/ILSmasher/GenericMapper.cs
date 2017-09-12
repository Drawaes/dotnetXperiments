using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;
using Mono.Collections.Generic;

namespace ILSmasher
{
    //public static class GenericMapper
    //{
    //    private static Queue<GenericParameterGenerator> _backlog = new Queue<GenericParameterGenerator>();

    //    public static int ItemsInBacklog => _backlog.Count;

    //    public static GenericParameter GetGenericParameter(GenericParameter oldParam, IGenericParameterProvider provider)
    //    {
    //        var paramGen = new GenericParameterGenerator(oldParam, provider);
    //        paramGen.Finish();
    //        return paramGen.Defintion;
    //    }

    //    public static void GetGenericParameters(Collection<GenericParameter> old, Collection<GenericParameter> newParms, IGenericParameterProvider provider)
    //    {
    //        foreach (var p in old)
    //        {
    //            newParms.Add(GetGenericParameter(p,provider));
    //        }
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
    //            Console.WriteLine($"Generic type {item.Defintion.FullName}");
    //            item.Finish();

    //            counter++;
    //            count--;
    //        } while (count > 0 && _backlog.Count > 0);
    //        return counter;
    //    }
    //}
}

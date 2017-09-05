
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace ILSmasher
{
    public static class MethodMapper
    {
        private static Dictionary<string, MethodGenerator> _methods = new Dictionary<string, MethodGenerator>();
        private static Dictionary<string, MethodReference> _methodsToDereference = new Dictionary<string, MethodReference>();

        public static void AddMethod(MethodDefinition methodDef)
        {
            if (!_methods.ContainsKey(methodDef.SafeName()))
            {
                _methods.Add(methodDef.SafeName(), new MethodGenerator(methodDef));
            }
        }

        public static string SafeName(this MethodReference methodReference) => $"{methodReference.DeclaringType.SafeName()}.{methodReference.Name}";

        internal static MethodDefinition GetMethodDefinition(MethodReference methodRef)
        {
            var className = methodRef.DeclaringType.SafeName();
            var type = TypeMapper.GetType(className);
            var count = type.AllMethods().Where(m => MatchMethod(m, methodRef)).ToList();
            if(count.Count > 1)
            {
                //hack
                //count = count.Where(f => f.MetadataToken == methodRef.MetadataToken).ToList();
                return count.First(m => m.FullName == methodRef.FullName);
            }
            return type.AllMethods().Single(m => MatchMethod(m, methodRef));
        }

        

        public static bool MatchMethod(MethodDefinition def, MethodReference methodRef)
        {
            if(methodRef is GenericInstanceMethod genMethod)
            {
                methodRef = genMethod.ElementMethod;
                return MatchMethod(def, methodRef);
            }
            
            //Need more complex matching rules this is a placeholder
            if (def.Name != methodRef.Name)
            {
                return false;
            }
            if (def.HasParameters != methodRef.HasParameters)
            {
                return false;
            }
            if (def.HasParameters)
            {
                if (def.Parameters.Count != methodRef.Parameters.Count)
                {
                    return false;
                }
                for (var i = 0; i < def.Parameters.Count; i++)
                {
                    if(!MatchParameters(def.Parameters[i], methodRef.Parameters[i]))
                    {
                        return false;
                    }
                }
            }
            if(def.HasGenericParameters != methodRef.HasGenericParameters)
            {
                return false;
            }
            if(def.HasGenericParameters)
            {
                if(def.GenericParameters.Count != methodRef.GenericParameters.Count)
                {
                    return false;
                }
                
            }
            return true;
        }

        public static bool MatchParameters(ParameterReference parm1, ParameterReference parm2)
        {
            TypeDefinition td;

            if (parm1.Index != parm2.Index)
            {
                return false;
            }
            if(parm1.ParameterType.ContainsGenericParameter || parm2.ParameterType.ContainsGenericParameter)
            {
                return true;
            }
            return MatchTypeReference(parm1.ParameterType, parm2.ParameterType);
        }

        public static bool MatchTypeReference(TypeReference type1, TypeReference type2) => type1.SafeName() == type2.SafeName();

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace ILSmasher
{
    public static class TypeExtensions
    {
        public static string SafeName(this TypeReference reference)
        {
            if (reference.IsNested)
            {
                return $"{reference.DeclaringType.SafeName()}.{reference.Name}";
            }
            var nameSpace = reference.SafeNamespace();
            return $"{nameSpace}.{reference.Name}";
        }

        public static string SafeNamespace(this TypeReference reference)
        {
            if (string.IsNullOrWhiteSpace(reference.Namespace))
            {
                return reference.Module.Name;
            }
            return reference.Namespace;
        }

        public static Tuple<MetadataToken, string> GetKey(this TypeDefinition typeDef) => Tuple.Create(typeDef.MetadataToken, typeDef.SafeNamespace());


        //public static string SafeName(this MethodReference reference)
        //{
        //    var nameSpace = string.IsNullOrWhiteSpace(reference.MetadataToken)
        //}
    }
}

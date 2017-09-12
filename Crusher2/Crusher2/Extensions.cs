using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace Crusher2
{
    public static class Extensions
    {
        public static Key GetKey(this TypeReference def) => new Key(def.MetadataToken, def.Scope);
        public static Key GetKey(this MethodDefinition def) => new Key(def.MetadataToken, def.DeclaringType.Scope);
        public static Key GetKey(this FieldDefinition def) => new Key(def.MetadataToken, def.DeclaringType.Scope);
        public static Key GetKey(this EventDefinition def) => new Key(def.MetadataToken, def.DeclaringType.Scope);
        public static Key GetKey(this ParameterDefinition def) => new Key(def.MetadataToken, ((MethodReference)def.Method).DeclaringType.Scope);
        public static Key GetKey(this GenericParameter def) => new Key(def.MetadataToken, def.Scope);
        public static string GetStringKey(this TypeDefinition def) => $"{def.GetSafeNamespace()}.{def.Name}";
        public static string GetStringKey(this MethodDefinition def) => $"{def.DeclaringType.GetStringKey()}.{def.Name}";


        public static string GetSafeNamespace(this TypeReference typeRef)
        {
            if (typeRef.DeclaringType == null)
            {
                if (string.IsNullOrWhiteSpace(typeRef.Namespace))
                {
                    return typeRef.Module.Name;
                }
                return typeRef.Namespace;
            }
            return typeRef.Namespace;
        }
    }
}

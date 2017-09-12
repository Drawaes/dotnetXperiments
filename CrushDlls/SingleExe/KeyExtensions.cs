using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace SingleExe
{
    public static class KeyExtensions
    {
        public static Key GetKey(this TypeDefinition def) => new Key(def.MetadataToken, def.Scope);
        public static Key GetKey(this MethodDefinition def) => new Key(def.MetadataToken, def.DeclaringType.Scope);

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

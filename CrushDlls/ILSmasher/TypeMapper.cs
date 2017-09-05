using System;
using System.Collections.Generic;
using Mono.Cecil;

namespace ILSmasher
{
    public static class TypeMapper
    {
        private static Dictionary<string, TypeGenerator> _typeDef = new Dictionary<string, TypeGenerator>();
        
        public static void AddType(TypeDefinition typeDef)
        {
            if (!_typeDef.TryGetValue(typeDef.SafeName(), out TypeGenerator typeGen))
            {
                typeGen = new TypeGenerator(typeDef);
                _typeDef[typeGen.Name] = typeGen;
            }
            else
            {
                typeGen.AddOldTypeDef(typeDef);
            }
            
            foreach(var innerType in typeDef.NestedTypes)
            {
                AddType(innerType);
            }
        }

        public static void TouchType(TypeReference type)
        {
            if(type.IsGenericParameter)
            {
                return;
            }
            var name = type.SafeName();
            _typeDef[name].IsReferenced = true;
        }

        public static void PrintTypeList(string outputPath)
        {
            System.IO.File.WriteAllLines(outputPath, _typeDef.Keys);
        }

        internal static TypeGenerator GetType(string className)
        {
            return _typeDef[className];
        }

        public static void BuildTypes()
        {
            foreach (var t in _typeDef)
            {
                
            }
        }


    }
}

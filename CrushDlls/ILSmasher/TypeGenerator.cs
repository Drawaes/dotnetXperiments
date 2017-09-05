using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace ILSmasher
{
    public class TypeGenerator
    {
        private List<TypeDefinition> _oldTypeDefs = new List<TypeDefinition>();
        private string _uniqueName;

        public TypeGenerator(TypeDefinition typeDef)
        {
            _uniqueName = typeDef.SafeName();
            _oldTypeDefs.Add(typeDef);
        }

        public string Name => _uniqueName;
        public bool IsReferenced { get; set; }

        public void AddOldTypeDef(TypeDefinition typeDef)
        {
            if (_oldTypeDefs.Any(td => td.GetKey() != typeDef.GetKey()))
            {
                return;
            }
            _oldTypeDefs.Add(typeDef);
        }

        public IEnumerable<MethodDefinition> AllMethods() => _oldTypeDefs.SelectMany(td => td.Methods);
    }
}

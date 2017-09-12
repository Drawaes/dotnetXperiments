using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace Crusher2.Strike2.Compare
{
    public class CompareParameter : IEqualityComparer<ParameterDefinition>
    {
        private ModuleRebuilder _builder;

        public CompareParameter(ModuleRebuilder builder) => _builder = builder;

        public bool Equals(ParameterDefinition x, ParameterDefinition y)
        {
            if ( x == null && y == null) return true;
            if (x == null) return false;

            if (x.Name != y.Name) return false;

            if(x.ParameterType.Name != y.ParameterType.Name
                || x.ParameterType.Namespace != y.ParameterType.Namespace)
            {
                return false;
            }
            return true;
        }

        public int GetHashCode(ParameterDefinition obj)
        {
            throw new NotImplementedException();
        }
    }
}

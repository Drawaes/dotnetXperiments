using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace Crusher2.Strike2.Compare
{
    public class CompareMethod : IEqualityComparer<MethodDefinition>
    {
        private ModuleRebuilder _builder;
        private CompareParameter _compareParameter;
        private CompareGenericParameter _compareGenericParameter;

        public CompareMethod(ModuleRebuilder builder)
        {
            _builder = builder;
            _compareParameter = new CompareParameter(builder);
            _compareGenericParameter = new CompareGenericParameter(builder);
        }

        public bool Equals(MethodDefinition x, MethodDefinition y)
        {
            if (x == null && y == null) return true;
            if (x == null) return false;

            if (x.GetStringKey() != y.GetStringKey())
            {
                return false;
            }

            if (x.Parameters.Count != y.Parameters.Count) return false;
            if (x.GenericParameters.Count != y.GenericParameters.Count) return false;
            if (x.Attributes != y.Attributes) return false;

            for (var i = 0; i < x.Parameters.Count;i++)
            {
                if(!_compareParameter.Equals(x.Parameters[i], y.Parameters[i]))
                {
                    return false;
                }
            }

            for(var i = 0; i< x.GenericParameters.Count;i++)
            {
                if(! _compareGenericParameter.Equals(x.GenericParameters[i], y.GenericParameters[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public int GetHashCode(MethodDefinition obj)
        {
            throw new NotImplementedException();
        }
    }
}

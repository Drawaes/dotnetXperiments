using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace Crusher2.Strike2.Compare
{
    public class CompareGenericParameter : IEqualityComparer<GenericParameter>
    {
        private ModuleRebuilder _builder;

        public CompareGenericParameter(ModuleRebuilder builder) => _builder = builder;

        public bool Equals(GenericParameter x, GenericParameter y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;

            if (x.Name != y.Name) return false;

            throw new NotImplementedException();
        }

        public int GetHashCode(GenericParameter obj)
        {
            throw new NotImplementedException();
        }
    }
}

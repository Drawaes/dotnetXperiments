using System;
using System.Collections.Generic;
using System.Text;

namespace Crusher2.Strike2
{
    public abstract class MapperBase
    {
        private ModuleRebuilder _builder;

        public MapperBase(ModuleRebuilder builder) => _builder = builder;

        public ModuleRebuilder Builder => _builder;
    }
}

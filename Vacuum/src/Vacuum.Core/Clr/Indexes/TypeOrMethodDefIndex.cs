using System;
using System.Collections.Generic;
using System.Text;
using Vacuum.Core.Clr.Flags;

namespace Vacuum.Core.Clr.Indexes
{
    public class TypeOrMethodDefIndex : MultiIndex<TypeOrMethodDefFlag>
    {
        protected override byte _bitShift => 1;
    }
}

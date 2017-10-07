using System;
using System.Collections.Generic;
using System.Text;
using Vacuum.Core.Clr.Flags;

namespace Vacuum.Core.Clr.Indexes
{
    public class HasCustomAttributeIndex : MultiIndex<HasCustomAttributeFlag>
    {
        protected override byte _bitShift => 5;
    }
}

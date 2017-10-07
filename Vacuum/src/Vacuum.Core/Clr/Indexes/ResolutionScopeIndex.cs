using System;
using System.Collections.Generic;
using System.Text;
using Vacuum.Core.Clr.Flags;
using Vacuum.Core.Clr.Rows;

namespace Vacuum.Core.Clr.Indexes
{
    public class ResolutionScopeIndex : MultiIndex<ResolutionScopeFlag>
    {
        protected override byte _bitShift => 2;
    }
}

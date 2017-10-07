using System;
using System.Collections.Generic;
using System.Text;

namespace Vacuum.Core.Clr.Indexes
{
    public abstract class SimpleIndex : Index
    {
        public int Index => (int)_rawIndex & 0x00FF_FFFF;
    }
}

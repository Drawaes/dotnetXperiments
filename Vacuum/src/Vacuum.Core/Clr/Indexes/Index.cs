using System;
using System.Collections.Generic;
using System.Text;

namespace Vacuum.Core.Clr.Indexes
{
    public abstract class Index
    {
        protected uint _rawIndex;

        internal void SetRawIndex(uint index) => _rawIndex = index;

        internal abstract void Resolve(ClrData clrData);
    }
}

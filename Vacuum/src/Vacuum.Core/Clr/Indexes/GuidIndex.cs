using System;
using System.Collections.Generic;
using System.Text;

namespace Vacuum.Core.Clr.Indexes
{
    public class GuidIndex:SimpleIndex
    {
        private Guid? _value;

        public Guid? Value => _value;

        internal override void Resolve(ClrData clrData) => _value = clrData.Guids.GetGuid(_rawIndex);
    }
}

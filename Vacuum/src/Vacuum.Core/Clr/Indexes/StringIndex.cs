using System;
using System.Collections.Generic;
using System.Text;

namespace Vacuum.Core.Clr.Indexes
{
    public class StringIndex : SimpleIndex
    {
        private string _value;
        public string Value => _value;

        internal override void Resolve(ClrData clrData) => _value = clrData.Strings.GetString(_rawIndex);
    }
}

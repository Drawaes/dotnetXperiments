using System;
using System.Collections.Generic;
using System.Text;
using Vacuum.Core.Clr.Flags;
using Vacuum.Core.Clr.Rows;

namespace Vacuum.Core.Clr.Indexes
{
    public abstract class MultiIndex<T> : SingleIndex where T : struct
    {
        protected byte _bitMask => (byte)((1 << _bitShift) - 1);
        protected abstract byte _bitShift { get; }
        protected Row _row;

        internal override void Resolve(ClrData clrData)
        {
            if(_rawIndex == 0)
            {
                return;
            }
            var flag = Enum.Parse<TableFlag>(Enum.GetName(typeof(T),_rawIndex & _bitMask));
            var index = (int)(_rawIndex >> _bitShift);
            _row = clrData.GetTable(flag).GetRow(index);
        }
    }
}

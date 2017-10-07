using System;
using System.Collections.Generic;
using System.Text;
using Vacuum.Core.Clr.Rows;

namespace Vacuum.Core.Clr
{
    public interface IClrTable
    {
        int Count { get; }

        void LoadRows(ref ClrMetaReader reader);
        void Resolve(ClrData parentData);
        Row GetRow(int rowId);
    }
}

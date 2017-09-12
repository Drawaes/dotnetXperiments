using System;
using System.Collections.Generic;
using System.Text;

namespace PEQuick
{
    [Flags]
    public enum HeapOffsetSizeFlags : byte
    {
        String = 0x01,
        GUID = 0x02,
        Blob = 0x04,
    }
}

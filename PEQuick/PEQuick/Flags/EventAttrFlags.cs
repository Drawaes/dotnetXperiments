using System;
using System.Collections.Generic;
using System.Text;

namespace PEQuick.Flags
{
    [Flags]
    public enum EventAttrFlags:ushort
    {
        evSpecialName = 0x0200,
        evReservedMask = 0x0400,
        evRTSpecialName = 0x0400,
    }
}

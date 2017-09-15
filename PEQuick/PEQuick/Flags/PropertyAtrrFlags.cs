using System;
using System.Collections.Generic;
using System.Text;

namespace PEQuick.Flags
{
    public enum PropertyAtrrFlags : ushort
    {
        prSpecialName = 0x0200,     // property is special. Name describes how.

        // Reserved flags for Runtime use only.
        prReservedMask = 0xf400,
        prRTSpecialName = 0x0400,     // Runtime(metadata internal APIs) should check name encoding.
        prHasDefault = 0x1000,
        prUnused = 0xe9ff,
    }
}

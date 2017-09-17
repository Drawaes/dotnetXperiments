using System;
using System.Collections.Generic;
using System.Text;

namespace PEQuick.Flags
{
    public enum MethodBodyFormat:byte
    {
        Mask = 0x03,
        Tiny = 0x02,
        Fat = 0x03,
    }
}

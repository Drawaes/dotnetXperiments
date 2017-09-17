using System;
using System.Collections.Generic;
using System.Text;

namespace PEQuick.Flags
{
    public enum PEFormatType : ushort
    {
        PE32 = 0x10b,
        PE32Plus = 0x20b,
    }
}

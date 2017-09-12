using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace PEQuick
{
    [StructLayout(LayoutKind.Sequential, Pack =1)]
    public struct ImageDataDirectory
    {
        public uint VirtualAddress;
        public uint Size;
    }
}

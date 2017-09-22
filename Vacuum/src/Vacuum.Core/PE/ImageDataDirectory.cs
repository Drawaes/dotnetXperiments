using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Vacuum.Core.PE
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ImageDataDirectory
    {
        public int VirtualAddress;
        public int Size;
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Vacuum.Core.Clr.Flags;

namespace Vacuum.Core.Clr
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ClrMetaDataHeader
    {
        public uint Reserved;
        public byte MajorVersion;
        public byte MinorVersion;
        public HeapOffsetSizeFlags HeapOffsets;
        public byte Reserved2;
        public ulong EnabledTables;
        public ulong SortedTables;
    }
}

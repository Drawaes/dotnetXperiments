using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using PEQuick.Flags;

namespace PEQuick.PE
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PEOptions
    {
        public PEFormatType Bitness;
        public byte LMajor;
        public byte LMinor;
        public uint CodeSize;
        public uint InitializedDataSize;
        public uint UninitializedDataSize;
        public uint EntryPointRVA;
        public uint BaseOfCode;
        public ulong ImageBase;
        public uint SectionAlignment;
        public uint FileAlignment;
        public ushort OSMajor;
        public ushort OSMinor;
        public ushort UserMajor;
        public ushort UserMinor;
        public ushort SubSysMajor;
        public ushort SubSysMinor;
        public uint Reserved;
        public uint ImageSize;
        public uint HeaderSize;
        public uint FileChecksum;
        public ushort SubSystem;
        public DllFlags DllFlags;

        public bool Is64 => Bitness == PEFormatType.PE32Plus;
    }
}

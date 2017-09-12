using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace PEQuick
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Section
    {
        public ulong Name;
        public uint VirtualSize;
        public uint VirtualAddress;
        public uint SizeOfRawData;
        public uint PointerToRawData;
        public uint PointerToRelocations;
        public uint PointerToLineNumbers;
        public uint NumberOfRelocations;
        public uint NumberOfLineNumbers;
        public uint Characteristics;

        public uint VirtualEnd => VirtualAddress + SizeOfRawData;
        public uint DevirtualisedAddress => PointerToRawData - VirtualAddress;
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Vacuum.Core.PE
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SectionHeader
    {
        public ulong Name;
        public int VirtualSize;
        public int VirtualAddress;
        public int SizeOfRawData;
        public int PointerToRawData;
        public int PointerToRelocations;
        public int PointerToLineNumbers;
        public ushort NumberOfRelocations;
        public ushort NumberOfLineNumbers;
        public uint Characteristics;

        public int VirtualEnd => VirtualAddress + SizeOfRawData;
        public int DevirtualisedAddress => PointerToRawData - VirtualAddress;

        public override string ToString()
        {
            return $"{VirtualAddress}-{VirtualAddress + SizeOfRawData}";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using PEQuick.Flags;

namespace PEQuick.PE
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PEHeader
    {
        public MachineType MachineType;
        public ushort NumberOfSections;
        public uint TimeStamp;
        public uint OffsetToSymbolTable;
        public uint NumberOfSymbols;
        public ushort SizeOfOptionHeader;
        public PECharacteristicFlags Characteristics;
    }
}

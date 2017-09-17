using System;
using System.Collections.Generic;
using System.Text;

namespace PEQuick
{
    public static class MagicNumbers
    {
        public const ushort DosMagicNumber = 0x5a4d;
        public const uint PEMagicNumber = 0x00004550;
        public const ushort x64Bitness = 0x020b;
        public const byte PEStartOffset = 0x3c;
        public const uint MetaData = 0x424a5342;

        public static ulong[] DosHeader = new ulong[]
        {
            0x4d5A009000030000,
            0x00040000FFFF0000,
            0x00b8000000000000,
            0x0040000000000000,
            0x0000000000000000,
            0x0000000000000000,
            0x0000000000000000,
            0x0000000000000080,
        };
    }
}

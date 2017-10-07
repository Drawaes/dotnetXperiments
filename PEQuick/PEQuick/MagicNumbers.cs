﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PEQuick
{
    public static class MagicNumbers
    {
        public const ushort x64Bitness = 0x020b;
        

        public static byte[] DosHeader = new byte[]
        {
            0x4d,0x5A,0x90,0x00,0x03,0x00,0x00,0x00,
            0x04,0x00,0x00,0x00,0xFF,0xFF,0x00,0x00,
            0xb8,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x40,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x80,0x00,0x00,0x00,
        };

        public static byte[] MSDosWarning = new byte[]
        {
            0x0E,0x1F,0xBA,0x0E,0x00,0xB4,0x09,0xCD,
            0x21,0xB8,0x01,0x4C,0xCD,0x21,0x54,0x68,
            0x69,0x73,0x20,0x70,0x72,0x6F,0x67,0x72,
            0x61,0x6D,0x20,0x63,0x61,0x6E,0x6E,0x6F,
            0x74,0x20,0x62,0x65,0x20,0x72,0x75,0x6E,
            0x20,0x69,0x6E,0x20,0x44,0x4F,0x53,0x20,
            0x6D,0x6F,0x64,0x65,0x2E,0x0D,0x0D,0x0A,
            0x24,0x00,0x00,0x00,0x00,0x00,0x00,0x00
        };
    }
}

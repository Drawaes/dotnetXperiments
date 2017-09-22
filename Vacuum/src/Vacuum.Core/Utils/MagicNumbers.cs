using System;
using System.Collections.Generic;
using System.Text;

namespace Vacuum.Core.Utils
{
    public class MagicNumbers
    {
        public const ushort DosMagicNumber = 0x5a4d;
        public const uint PEMagicNumber = 0x00004550;
        public const byte PEStartOffset = 0x3c;
        public const uint ClrMetaDataMagicNumber = 0x424a5342;
    }
}

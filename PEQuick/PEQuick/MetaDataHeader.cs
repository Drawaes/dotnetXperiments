using System;
using System.Collections.Generic;
using System.Text;

namespace PEQuick
{
    public class MetaDataHeader
    {
        public ushort MajorVersion;
        public ushort MinorVersion;
        public uint Reserved;
        public string Version;
        public ushort Flags;
        public ushort Streams;
    }
}

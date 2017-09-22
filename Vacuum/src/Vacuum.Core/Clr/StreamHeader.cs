using System;
using System.Collections.Generic;
using System.Text;

namespace Vacuum.Core.Clr
{
    public class StreamHeader
    {
        public uint Offset { get; set; }
        public uint Size { get; set; }
        public string Name { get; set; }

        public StreamHeader(ref ClrReader spanReader)
        {
            Offset = spanReader.Read<uint>();
            Size = spanReader.Read<uint>();
            Name = spanReader.ReadAlignedString();
        }
    }
}

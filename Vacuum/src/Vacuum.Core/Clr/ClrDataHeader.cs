using System;
using System.Collections.Generic;
using System.Text;


namespace Vacuum.Core.Clr
{
    public class ClrDataHeader
    {
        public ushort MajorVersion { get; set; }
        public ushort MinorVersion { get; set; }
        public uint Reserved { get; set; }
        public string Version { get; set; }
        public ushort Flags { get; set; }
        public ushort Streams { get; set; }

        public ClrDataHeader(ref ClrReader reader)
        {
            reader.CheckMagicNumber(Utils.MagicNumbers.ClrMetaDataMagicNumber);
            MajorVersion = reader.Read<ushort>();
            MinorVersion = reader.Read<ushort>();
            Reserved = reader.Read<uint>();
            var stringLength = reader.Read<int>();
            Version = reader.ReadFixedLengthAscii(stringLength);
            Flags = reader.Read<ushort>();
            Streams = reader.Read<ushort>();
        }
    }
}

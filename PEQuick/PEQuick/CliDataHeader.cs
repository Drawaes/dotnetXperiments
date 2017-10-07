using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PEQuick
{
    public class CliDataHeader
    {
        public ushort MajorVersion;
        public ushort MinorVersion;
        public uint Reserved;
        public string Version;
        public ushort Flags;
        public ushort Streams;
        private uint StringLength => Utils.Align((uint)Version.Length + 1, 4);
        public uint Size => StringLength + 4 + sizeof(ushort) * 4 + sizeof(uint) + 4;

        internal Span<byte> GetSpan()
        {
            var buffer = new byte[Size];
            var span = new Span<byte>(buffer);
            span = span.Write(MagicNumbers.Cl);
            span = span.Write(MajorVersion);
            span = span.Write(MinorVersion);
            span = span.Write(Reserved);
            span = span.Write((uint)(Version.Length + 1));
            span = span.WriteAlignedString(Version);
            span = span.Write(Flags);
            span = span.Write(Streams);
            Debug.Assert(span.Length == 0);
            return new Span<byte>(buffer);
        }
    }
}

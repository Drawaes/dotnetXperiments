using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PEQuick
{
    public struct StreamHeader
    {
        public uint Offset;
        public uint Size;
        public string Name;

        public uint GetSize() => Utils.Align((uint)Name.Length + 1, 4) + 8;

        public Span<byte> GetSpan()
        {
            var buffer = new byte[GetSize()];
            var span = new Span<byte>(buffer);
            span = span.Write(Offset);
            span = span.Write(Size);
            span = span.WriteAlignedString(Name);

            Debug.Assert(span.Length == 0);
            return new Span<byte>(buffer);
        }
    }
}

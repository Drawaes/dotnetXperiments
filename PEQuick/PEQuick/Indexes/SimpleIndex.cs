using System;
using System.Collections.Generic;
using System.Text;

namespace PEQuick.Indexes
{
    public abstract class SimpleIndex : Index
    {
        public abstract uint TableOffset { get; }

        internal override Span<byte> Write(Span<byte> input, Dictionary<uint, uint> remapper, bool largeFormat)
        {
            var tag = _rawIndex | TableOffset;
            if (remapper.TryGetValue(tag, out uint newTag))
            {
                tag = newTag;
            }
            tag = tag & 0x00FF_FFFF;
            if (largeFormat)
            {
                input = input.Write(tag);
            }
            else
            {
                input = input.Write((ushort)tag);
            }
            return input;
        }
    }
}

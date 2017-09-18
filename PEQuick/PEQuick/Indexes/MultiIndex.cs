using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public abstract class MultiIndex : Index
    {
        public abstract Row Row { get; }
        protected abstract byte BitMask { get; }
        protected abstract byte BitShift { get; }

        internal override Span<byte> Write(Span<byte> input, Dictionary<uint, uint> remapper, bool largeFormat)
        {
            uint index;
            if (Row == null)
            {
                index = 0;
            }
            else
            {
                if (remapper.TryGetValue(Row.Tag, out uint newTag))
                {
                    index = newTag;
                }
                else
                {
                    index = Row.Tag;
                }
            }
            //now we have the tag we need to remove the tag header
            index = index & 0x00FF_FFFF;
            //now shift it by the bit size
            index = index << BitShift;
            //finally add the tag on the bottom for the table type
            index = index | (_rawIndex & BitMask);
            if (largeFormat)
            {
                input = input.Write(index);
            }
            else
            {
                input = input.Write((ushort)index);
            }
            return input;
        }
    }
}

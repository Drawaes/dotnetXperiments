using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public abstract class SingleIndex : Index
    {
        public abstract Row Row { get; }
        
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

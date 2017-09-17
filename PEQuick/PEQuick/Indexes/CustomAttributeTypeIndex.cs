using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class CustomAttributeTypeIndex : Index
    {
        private Row _row;
        private const uint BitMask = 0b0000_0111;

        public uint Tag => ((uint)TableFlag.CustomAttribute << 24) | _rawIndex;

        internal override Span<byte> Write(Span<byte> input, Dictionary<uint, uint> remapper, bool largeFormat)
        {
            uint tag = _rawIndex;
            if (remapper.TryGetValue(Tag, out uint newTag))
            {
                tag = newTag & 0x00FF_FFFF;
            }
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

        internal override void Resolve(MetaDataTables tables)
        {
            var flags = _rawIndex & BitMask;
            var index = (int)(_rawIndex >> 3);
            switch (flags)
            {
                case 2:
                    _row = tables.GetCollection<MethodRow>()[index];
                    break;
                case 4:
                    _row = tables.GetCollection<MemberRefRow>()[index];
                    break;
            }
        }
    }
}

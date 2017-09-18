using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class MemberForwardedIndex : MultiIndex
    {
        private Row _row;

        protected override byte BitShift => 1;
        protected override byte BitMask => 0b0000_0001;
        public override Row Row => _row;

        internal override void Resolve(MetaDataTables tables)
        {
            var flags = _rawIndex & BitMask;
            var index = (int)(_rawIndex >> BitShift);
            switch (flags)
            {
                case 0:
                    _row = tables.GetCollection<FieldRow>()[index];
                    break;
                case 1:
                    _row = tables.GetCollection<MethodRow>()[index];
                    break;
            }
        }

        internal override Span<byte> Write(Span<byte> input, Dictionary<uint, uint> remapper, bool largeFormat)
        {
            throw new NotImplementedException();
        }
    }
}

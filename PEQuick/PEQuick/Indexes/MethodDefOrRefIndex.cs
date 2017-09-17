using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class MethodDefOrRefIndex : Index
    {
        private Row _row;
        private const uint BitMask = 0b0000_0001;

        public Row Row => _row;

        internal override void Resolve(MetaDataTables tables)
        {
            var flag = _rawIndex & BitMask;
            var index = (int)( _rawIndex >> 1);
            switch (flag)
            {
                case 0:
                    _row = tables.GetCollection<MethodRow>()[index];
                    break;
                case 1:
                    _row = tables.GetCollection<MemberRefRow>()[index];
                    break;
            }
        }

        internal override Span<byte> Write(Span<byte> input, Dictionary<uint, uint> remapper, bool largeFormat)
        {
            throw new NotImplementedException();
        }
    }
}

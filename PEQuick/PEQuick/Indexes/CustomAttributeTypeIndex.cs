using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class CustomAttributeTypeIndex : Index
    {
        private Row _row;
        private const uint BitMask = 0b0000_0111;

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

using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class CustomAttributeTypeIndex : MultiIndex
    {
        private Row _row;
        protected override byte BitMask => 0b0000_0111;
        protected override byte BitShift => 3;
        public override Row Row => _row;
                        
        internal override void Resolve(MetaDataTables tables)
        {
            var flags = _rawIndex & BitMask;
            var index = (int)(_rawIndex >> BitShift);
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

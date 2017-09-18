using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class HasSemanticsIndex : MultiIndex
    {
        private Row _row;
        protected override byte BitMask => 0b0000_0001;
        protected override byte BitShift => 1;
        public override Row Row => _row;

        internal override void Resolve(MetaDataTables tables)
        {
            var flag = _rawIndex & BitMask;
            var index = (int)(_rawIndex >> BitShift);
            switch (flag)
            {
                case 0:
                    _row = tables.GetCollection<EventRow>()[index];
                    break;
                case 1:
                    _row = tables.GetCollection<PropertyRow>()[index];
                    break;
            }
        }
    }
}

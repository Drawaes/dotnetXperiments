using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class HasSemanticsIndex : Index
    {
        private Row _row;
        private const uint BitMask = 0b0000_0001;

        internal override void Resolve(MetaDataTables tables)
        {
            var flag = _rawIndex & BitMask;
            var index = (int)(_rawIndex >> 1);
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

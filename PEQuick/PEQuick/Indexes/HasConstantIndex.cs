using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class HasConstantIndex : Index
    {
        private Row _row;
        private const uint BitMask = 0b0000_0011;

        public Row Row => _row;

        internal override void Resolve(MetaDataTables tables)
        {
            var flag = _rawIndex & BitMask;
            var index = (int)(_rawIndex >> 2);
            switch (flag)
            {
                case 0:
                    _row = tables.GetCollection<FieldRow>()[index];
                    break;
                case 1:
                    _row = tables.GetCollection<ParamRow>()[index];
                    break;
                case 2:
                    _row = tables.GetCollection<PropertyRow>()[index];
                    break;
            }
        }
    }
}

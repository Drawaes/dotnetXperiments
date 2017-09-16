using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class TypeOrMethodDefIndex : Index
    {
        private Row _row;

        public Row Row => _row;

        internal override void Resolve(MetaDataTables tables)
        {
            if(_rawIndex == 0)
            {
                return;
            }
            var flag = _rawIndex & 0b0000_0001;
            var index = (int)(_rawIndex >> 1);
            switch(flag)
            {
                case 0:
                    _row = tables.GetCollection<TypeDefRow>()[index];
                    break;
                case 1:
                    _row = tables.GetCollection<MethodRow>()[index];
                    break;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class TypeDefOrRefIndex : Index
    {
        private const uint BitMask = 0b0000_0011;
        private Row _row;

        public Row Row => _row;

        internal override void Resolve(MetaDataTables tables)
        {
            var flags = _rawIndex & BitMask;
            var index = (int)(_rawIndex >> 2);
            if (index == 0)
            {
                return;
            }
            switch (flags)
            {
                case 0:
                    _row = tables.GetCollection<TypeDefRow>()[index];
                    return;
                case 1:
                    _row = tables.GetCollection<TypeRefRow>()[index];
                    return;
                case 2:
                    _row = tables.GetCollection<TypeSpecRow>()[index];
                    return;
            }
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class HasFieldMarshalIndex : MultiIndex
    {
        public override Row Row => throw new NotImplementedException();
        protected override byte BitMask => throw new NotImplementedException();
        protected override byte BitShift => throw new NotImplementedException();

        internal override void Resolve(MetaDataTables tables)
        {
            throw new NotImplementedException();
        }

        /*
         * HasFieldMarshall: 1 bit to encode tag Tag
Field 0
Param 1
         */
    }
}

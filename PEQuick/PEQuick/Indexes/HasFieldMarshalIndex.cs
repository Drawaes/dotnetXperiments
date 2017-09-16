using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class HasFieldMarshalIndex : Index
    {
        public Row Row => throw new NotImplementedException();

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

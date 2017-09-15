using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class CustomAttributeTypeIndex : IIndex
    {
        private uint _rawIndex;

        public void Resolve(MetaDataTables tables)
        {
            throw new NotImplementedException();
        }

        public void SetRawIndex(uint rawIndex)
        {
            _rawIndex = rawIndex;
        }

        /*
         * CustomAttributeType: 3 bits to encode tag Tag
Not used 0
Not used 1
MethodDef 2
MemberRef 3
Not used 4

         */
    }
}

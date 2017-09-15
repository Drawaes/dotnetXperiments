using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class MethodDefOrRefIndex : IIndex
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
         * MethodDefOrRef: 1 bit to encode tag Tag
MethodDef 0
MemberRef 1
         */
    }
}

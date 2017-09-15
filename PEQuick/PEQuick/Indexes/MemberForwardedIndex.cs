using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class MemberForwardedIndex : IIndex
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
         * MemberForwarded: 1 bit to encode tag Tag
Field 0
MethodDef 1
         */
    }
}

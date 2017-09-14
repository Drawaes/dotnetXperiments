using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;

namespace PEQuick.Indexes
{
    public struct FieldIndex:IIndex
    {
        private uint _rawIndex;
                
        public uint Index => _rawIndex;

        public void SetRawIndex(uint rawIndex) => _rawIndex = rawIndex;
    }
}

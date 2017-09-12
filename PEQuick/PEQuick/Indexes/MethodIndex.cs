using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;

namespace PEQuick.Indexes
{
    public struct MethodIndex
    {
        private uint _rawIndex;

        public MethodIndex(ref MetaDataReader reader)
        {
            // TODO need to calculate the index size incase it is larger
            _rawIndex = reader.Read<ushort>();
        }
    }
}

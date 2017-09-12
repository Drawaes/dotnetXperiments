using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;

namespace PEQuick.Indexes
{
    public struct FieldIndex
    {
        private uint _rawIndex;

        public FieldIndex(ref MetaDataReader reader)
        {
            // TODO calculate the index size incase it is larger
            _rawIndex = reader.Read<ushort>();
        }

        public uint Index => _rawIndex;
    }
}

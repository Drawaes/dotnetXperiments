using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;

namespace PEQuick.Indexes
{
    public struct ResolutionScopeIndex
    {
        private uint _scopeIndex;

        public ResolutionScopeIndex(ref MetaDataReader reader) => _scopeIndex = reader.Read<ushort>();
    }
}

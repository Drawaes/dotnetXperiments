using System;
using System.Collections.Generic;
using System.Text;

namespace PEQuick.Indexes
{
    public struct TypeDefOrRefIndex
    {
        private uint _rawIndex;

        public TypeDefOrRefIndex(uint value)
        {
            // TODO calculate ref size
            _rawIndex = value;
        }
    }
}

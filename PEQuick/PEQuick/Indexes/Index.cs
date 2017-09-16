using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;

namespace PEQuick.Indexes
{
    public abstract class Index
    {
        protected uint _rawIndex;

        internal void SetRawIndex(uint rawIndex)
        {
            _rawIndex = rawIndex;
        }

        internal abstract void Resolve(MetaDataTables tables);
    }
}

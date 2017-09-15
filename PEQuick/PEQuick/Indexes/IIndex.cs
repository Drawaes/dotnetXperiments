using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public interface IIndex
    {
        void SetRawIndex(uint rawIndex);
        void Resolve(MetaDataTables tables);
    }
}

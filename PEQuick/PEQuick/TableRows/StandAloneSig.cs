using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public struct StandAloneSig : IRow
    {
        private BlobIndex _signature;

        public void Read(ref MetaDataReader reader)
        {
            _signature = reader.ReadIndex<BlobIndex>();
        }
    }
}

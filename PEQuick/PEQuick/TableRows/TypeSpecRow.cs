using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class TypeSpecRow : Row
    {
        private BlobIndex _signature;
        
        public override void Read(ref MetaDataReader reader)
        {
            _signature = reader.ReadIndex<BlobIndex>();
        }
    }
}

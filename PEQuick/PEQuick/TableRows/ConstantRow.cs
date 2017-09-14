using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public struct ConstantRow:IRow
    {
        private ushort _type;
        private HasConstantIndex _parent;
        private BlobIndex _value;
        
        public void Read(ref MetaDataReader reader)
        {
            _type = reader.Read<ushort>();
            _parent = reader.ReadIndex<HasConstantIndex>();
            _value = reader.ReadIndex<BlobIndex>();
        }
    }
}

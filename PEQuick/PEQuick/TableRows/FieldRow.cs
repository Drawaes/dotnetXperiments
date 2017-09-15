using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class FieldRow : Row
    {
        private ushort _flags;
        private StringIndex _nameIndex;
        private BlobIndex _signature;
        
        public override void Read(ref MetaDataReader reader)
        {
            _flags = reader.Read<ushort>();
            _nameIndex = reader.ReadIndex<StringIndex>();
            _signature = reader.ReadIndex<BlobIndex>();
        }

        public override string ToString() => _nameIndex.Value;
    }
}

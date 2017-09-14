using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public struct FieldMarshalRow : IRow
    {
        private HasFieldMarshalIndex _parent;
        private BlobIndex _nativeType;

        public void Read(ref MetaDataReader reader)
        {
            _parent = reader.ReadIndex<HasFieldMarshalIndex>();
            _nativeType = reader.ReadIndex<BlobIndex>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class PropertyRow : Row
    {
        private PropertyAtrrFlags _flags;
        private StringIndex _nameIndex;
        private BlobIndex _type;

        public override void Read(ref MetaDataReader reader)
        {
            _flags = reader.Read<PropertyAtrrFlags>();
            _nameIndex = reader.ReadIndex<StringIndex>();
            _type = reader.ReadIndex<BlobIndex>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class CustomAttributeRow : Row
    {
        private HasCustomAttributeIndex _parent;
        private CustomAttributeTypeIndex _type;
        private BlobIndex _value;

        public override void Read(ref MetaDataReader reader)
        {
            _parent = reader.ReadIndex<HasCustomAttributeIndex>();
            _type = reader.ReadIndex<CustomAttributeTypeIndex>();
            _value = reader.ReadIndex<BlobIndex>();
        }
    }
}

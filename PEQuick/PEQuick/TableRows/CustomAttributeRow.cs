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

        public override TableFlag Table => TableFlag.CustomAttribute;
        public override uint AssemblyTag => _parent.Row.AssemblyTag;

        public override void Resolve(MetaDataTables tables)
        {
            _parent.Resolve(tables);
            _type.Resolve(tables);
            _value.Resolve(tables);
        }

        public override void Read(ref MetaDataReader reader)
        {
            _parent = reader.ReadIndex<HasCustomAttributeIndex>();
            _type = reader.ReadIndex<CustomAttributeTypeIndex>();
            _value = reader.ReadIndex<BlobIndex>();
        }
    }
}

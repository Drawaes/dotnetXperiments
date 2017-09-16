using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class FieldMarshalRow : Row
    {
        private HasFieldMarshalIndex _parent;
        private BlobIndex _nativeType;

        public override TableFlag Table => TableFlag.FieldMarshal;
        public override uint AssemblyTag => _parent.Row.AssemblyTag;

        public override void Resolve(MetaDataTables tables)
        {
            _parent.Resolve(tables);
            _nativeType.Resolve(tables);
        }

        public override void Read(ref MetaDataReader reader)
        {
            _parent = reader.ReadIndex<HasFieldMarshalIndex>();
            _nativeType = reader.ReadIndex<BlobIndex>();
        }
    }
}

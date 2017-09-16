using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class ConstantRow : Row
    {
        private ushort _type;
        private HasConstantIndex _parent;
        private BlobIndex _value;

        public override TableFlag Table => TableFlag.Constant;
        public override uint AssemblyTag => _parent.Row.AssemblyTag;

        public override void Resolve(MetaDataTables tables)
        {
            _parent.Resolve(tables);
            _value.Resolve(tables);
        }

        public override void Read(ref MetaDataReader reader)
        {
            _type = reader.Read<ushort>();
            _parent = reader.ReadIndex<HasConstantIndex>();
            _value = reader.ReadIndex<BlobIndex>();
        }
    }
}

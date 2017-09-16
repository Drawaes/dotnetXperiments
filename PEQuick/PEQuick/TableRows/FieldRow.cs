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

        public override uint AssemblyTag => Parent.AssemblyTag;
        public override TableFlag Table => TableFlag.Field;

        public TypeDefRow Parent { get; internal set; }

        public override void Resolve(MetaDataTables tables)
        {
            _nameIndex.Resolve(tables);
            _signature.Resolve(tables);
        }

        public override void Read(ref MetaDataReader reader)
        {
            _flags = reader.Read<ushort>();
            _nameIndex = reader.ReadIndex<StringIndex>();
            _signature = reader.ReadIndex<BlobIndex>();
        }

        public override string ToString() => _nameIndex.Value;
    }
}

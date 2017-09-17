using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class FieldLayoutRow : Row
    {
        private uint _offset;
        private FieldIndex _field;

        public override TableFlag Table => TableFlag.FieldLayout;
        public override uint AssemblyTag => _field.Row.AssemblyTag;

        public override void Resolve(MetaDataTables tables)
        {
            _field.Resolve(tables);
        }

        public override void Read(ref MetaDataReader reader)
        {
            _offset = reader.Read<uint>();
            _field = reader.ReadIndex<FieldIndex>();
        }

        public override void WriteRow(ref MetaDataWriter writer, Dictionary<uint, uint> tokenRemapping)
        {
            throw new NotImplementedException();
        }
    }
}

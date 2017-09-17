using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class FieldRVA : Row
    {
        public uint RVA { get; set; }
        public FieldIndex Field { get; set; }

        public override TableFlag Table => TableFlag.FieldRVA;
        public override uint AssemblyTag => Field.Row.AssemblyTag;

        public override void Resolve(MetaDataTables tables)
        {
            Field.Resolve(tables);
        }

        public override void Read(ref MetaDataReader reader)
        {
            RVA = reader.Read<uint>();
            Field = reader.ReadIndex<FieldIndex>();
        }

        public override void WriteRow(ref MetaDataWriter writer, Dictionary<uint, uint> tokenRemapping)
        {
            throw new NotImplementedException();
        }
    }
}

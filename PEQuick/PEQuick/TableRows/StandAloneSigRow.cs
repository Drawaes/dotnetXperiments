using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class StandAloneSigRow : Row
    {
        private BlobIndex _signature;
        private AssemblyRow _row;

        public override TableFlag Table => TableFlag.StandAloneSig;
        public override uint AssemblyTag => _row.AssemblyTag;

        public override void Resolve(MetaDataTables tables)
        {
            _row = tables.GetCollection<AssemblyRow>()[1];
            _signature.Resolve(tables);
        }

        public override void Read(ref MetaDataReader reader)
        {
            _signature = reader.ReadIndex<BlobIndex>();
        }

        public override void WriteRow(ref MetaDataWriter writer, Dictionary<uint, uint> tokenRemapping)
        {
            throw new NotImplementedException();
        }
    }
}

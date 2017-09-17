using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class ManifestResourceRow : Row
    {
        private uint _offset;
        private uint _flags;
        private StringIndex _name;
        private ImplementationIndex _implementation;
        private AssemblyRow _parentRow;

        public override TableFlag Table => TableFlag.ManifestResource;
        public override uint AssemblyTag => _parentRow.AssemblyTag;

        public override void Resolve(MetaDataTables tables)
        {
            _parentRow = tables.GetCollection<AssemblyRow>()[1];
            _name.Resolve(tables);
            _implementation.Resolve(tables);
        }

        public override void Read(ref MetaDataReader reader)
        {
            _offset = reader.Read<uint>();
            _flags = reader.Read<uint>();
            _name = reader.ReadIndex<StringIndex>();
            _implementation = reader.ReadIndex<ImplementationIndex>();
        }

        public override void WriteRow(ref MetaDataWriter writer, Dictionary<uint, uint> tokenRemapping)
        {
            throw new NotImplementedException();
        }
    }
}

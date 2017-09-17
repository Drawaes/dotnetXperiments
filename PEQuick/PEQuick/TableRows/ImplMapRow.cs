using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class ImplMapRow : Row
    {
        private StringIndex _importName;
        private StringIndex _importScope;

        public ushort MappingFlags { get; set; }
        public MemberForwardedIndex MemberForwarded { get; set; }
        public override TableFlag Table => TableFlag.ImplMap;
        public override uint AssemblyTag => MemberForwarded.Row.AssemblyTag;

        public override void Resolve(MetaDataTables tables)
        {
            MemberForwarded.Resolve(tables);
            _importName.Resolve(tables);
            _importScope.Resolve(tables);
        }

        public override void Read(ref MetaDataReader reader)
        {
            MappingFlags = reader.Read<ushort>();
            MemberForwarded = reader.ReadIndex<MemberForwardedIndex>();
            _importName = reader.ReadIndex<StringIndex>();
            _importScope = reader.ReadIndex<StringIndex>();
        }

        public override void WriteRow(ref MetaDataWriter writer, Dictionary<uint, uint> tokenRemapping)
        {
            throw new NotImplementedException();
        }
    }
}

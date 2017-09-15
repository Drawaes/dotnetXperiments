using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class ImplMapRow : Row
    {
        public ushort MappingFlags;
        public MemberForwardedIndex MemberForwarded;
        private StringIndex _importName;
        private StringIndex _importScope;
        
        public override void Read(ref MetaDataReader reader)
        {
            MappingFlags = reader.Read<ushort>();
            MemberForwarded = reader.ReadIndex<MemberForwardedIndex>();
            _importName = reader.ReadIndex<StringIndex>();
            _importScope = reader.ReadIndex<StringIndex>();
        }
    }
}

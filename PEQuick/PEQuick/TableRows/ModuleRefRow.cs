using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class ModuleRefRow : Row
    {
        private StringIndex _nameIndex;

        public override TableFlag Table => TableFlag.ModuleRef;
        public override uint AssemblyTag => Tag;

        public override void Resolve(MetaDataTables tables)
        {
            _nameIndex.Resolve(tables);
        }
        
        public override void Read(ref MetaDataReader reader)
        {
            _nameIndex = reader.ReadIndex<StringIndex>();
        }

        public override void WriteRow(ref MetaDataWriter writer, Dictionary<uint, uint> tokenRemapping)
        {
            throw new NotImplementedException();
        }
    }
}

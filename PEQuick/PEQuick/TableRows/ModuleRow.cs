using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class ModuleRow : Row
    {
        private ushort _generation;
        private StringIndex _nameIndex;
        private uint _mvid;
        private uint _encId;
        private uint _encBaseId;
        private AssemblyRow _parent;

        public override TableFlag Table => TableFlag.Module;
        public override uint AssemblyTag => _parent.AssemblyTag;

        public override void Resolve(MetaDataTables tables)
        {
            _parent = tables.GetCollection<AssemblyRow>()[1];
            _nameIndex.Resolve(tables);
        }

        public override void Read(ref MetaDataReader reader)
        {
            _generation = reader.Read<ushort>();
            _nameIndex = reader.ReadIndex<StringIndex>();
            _mvid = reader.ReadGuidOffset();
            _encId = reader.ReadGuidOffset();
            _encBaseId = reader.ReadGuidOffset();
        }
    }
}
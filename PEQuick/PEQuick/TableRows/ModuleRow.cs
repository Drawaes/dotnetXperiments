using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using PEQuick.Flags;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class ModuleRow : Row
    {
        private StringIndex _nameIndex;
        private GuidIndex _mvid;
        private GuidIndex _encId;
        private GuidIndex _encBaseId;
        private AssemblyRow _parent;

        public override TableFlag Table => TableFlag.Module;
        public override uint AssemblyTag => _parent.AssemblyTag;

        public override void Resolve(MetaDataTables tables)
        {
            _parent = tables.GetCollection<AssemblyRow>()[1];
            _nameIndex.Resolve(tables);
        }
        
        public override void WriteRow(ref MetaDataWriter writer, Dictionary<uint, uint> tokenRemapping)
        {
            writer.Write(_generation);
            writer.WriteIndex(_nameIndex);
            writer.WriteIndex(_mvid);
            writer.WriteIndex(_encId);
            writer.WriteIndex(_encBaseId);
        }
    }
}
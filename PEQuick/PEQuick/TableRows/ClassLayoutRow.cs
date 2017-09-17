using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class ClassLayoutRow : Row
    {
        private ushort _packingSize;
        private uint _classSize;
        private TypeDefIndex _parent;

        public override TableFlag Table => TableFlag.ClassLayout;
        public override uint AssemblyTag => _parent.Row.AssemblyTag;

        public override void Resolve(MetaDataTables tables)
        {
            _parent.Resolve(tables);
        }

        public override void Read(ref MetaDataReader reader)
        {
            _packingSize = reader.Read<ushort>();
            _classSize = reader.Read<uint>();
            _parent = reader.ReadIndex<TypeDefIndex>();
        }

        public override void WriteRow(ref MetaDataWriter writer, Dictionary<uint, uint> tokenRemapping)
        {
            throw new NotImplementedException();
        }
    }
}

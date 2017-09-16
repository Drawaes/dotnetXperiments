using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class GenericParamRow : Row
    {
        private ushort _number;
        private ushort _flags;
        private TypeOrMethodDefIndex _owner;
        private StringIndex _name;

        public override uint AssemblyTag => _owner.Row.AssemblyTag;
        public override TableFlag Table => TableFlag.GenericParam;

        public override void Resolve(MetaDataTables tables)
        {
            _owner.Resolve(tables);
            _name.Resolve(tables);
        }

        public override void Read(ref MetaDataReader reader)
        {
            _number = reader.Read<ushort>();
            _flags = reader.Read<ushort>();
            _owner = reader.ReadIndex<TypeOrMethodDefIndex>();
            _name = reader.ReadIndex<StringIndex>();
        }
    }
}

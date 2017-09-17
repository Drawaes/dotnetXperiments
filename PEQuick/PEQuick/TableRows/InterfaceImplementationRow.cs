using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class InterfaceImplementationRow : Row
    {
        private TypeDefIndex _typeDef;
        private TypeDefOrRefIndex _interface;

        public override TableFlag Table => TableFlag.InterfaceImpl;
        public override uint AssemblyTag => _typeDef.Row.AssemblyTag;

        public override void Resolve(MetaDataTables tables)
        {
            _typeDef.Resolve(tables);
            _interface.Resolve(tables);
        }

        public override void Read(ref MetaDataReader reader)
        {
            _typeDef = reader.ReadIndex<TypeDefIndex>();
            _interface = reader.ReadIndex<TypeDefOrRefIndex>();
        }

        public override void WriteRow(ref MetaDataWriter writer, Dictionary<uint, uint> tokenRemapping)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class MethodSemanticsRow : Row
    {
        private ushort _semantics;
        private MethodIndex _method;
        private HasSemanticsIndex _association;

        public override TableFlag Table => TableFlag.MethodSemantics;
        public override uint AssemblyTag => _method.Row.AssemblyTag;

        public override void Resolve(MetaDataTables tables)
        {
            _method.Resolve(tables);
            _association.Resolve(tables);
        }

        public override void Read(ref MetaDataReader reader)
        {
            _semantics = reader.Read<ushort>();
            _method = reader.ReadIndex<MethodIndex>();
            _association = reader.ReadIndex<HasSemanticsIndex>();
        }

        public override void WriteRow(ref MetaDataWriter writer, Dictionary<uint, uint> tokenRemapping)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class MethodImplRow : Row
    {
        private TypeDefIndex _class;
        private MethodDefOrRefIndex _methodBody;
        private MethodDefOrRefIndex _methodDeclaration;

        public override TableFlag Table => TableFlag.MethodImpl;
        public override uint AssemblyTag => _class.Row.AssemblyTag;

        public override void Resolve(MetaDataTables tables)
        {
            _class.Resolve(tables);
            _methodBody.Resolve(tables);
            _methodDeclaration.Resolve(tables);
        }

        public override void Read(ref MetaDataReader reader)
        {
            _class = reader.ReadIndex<TypeDefIndex>();
            _methodBody = reader.ReadIndex<MethodDefOrRefIndex>();
            _methodDeclaration = reader.ReadIndex<MethodDefOrRefIndex>();

        }

        public override void WriteRow(ref MetaDataWriter writer, Dictionary<uint, uint> tokenRemapping)
        {
            throw new NotImplementedException();
        }
    }
}

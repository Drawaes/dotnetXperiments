using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class NestedClassRow : Row
    {
        private TypeDefIndex _nestedClass;
        private TypeDefIndex _enclosingClass;

        public override TableFlag Table => TableFlag.NestedClass;
        public override uint AssemblyTag => _enclosingClass.Row.AssemblyTag;

        public override void Resolve(MetaDataTables tables)
        {
            _nestedClass.Resolve(tables);
            _enclosingClass.Resolve(tables);
        }

        public override void Read(ref MetaDataReader reader)
        {
            _nestedClass = reader.ReadIndex<TypeDefIndex>();
            _enclosingClass = reader.ReadIndex<TypeDefIndex>();
        }
    }
}

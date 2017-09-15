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

        public override void Read(ref MetaDataReader reader)
        {
            _nestedClass = reader.ReadIndex<TypeDefIndex>();
            _enclosingClass = reader.ReadIndex<TypeDefIndex>();
        }
    }
}

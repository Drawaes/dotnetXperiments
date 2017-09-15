using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class InterfaceImplementationRow : Row
    {
        private TypeDefIndex _typeDef;
        private TypeDefOrRefIndex _interface;

        public override void Read(ref MetaDataReader reader)
        {
            _typeDef = reader.ReadIndex<TypeDefIndex>();
            _interface = reader.ReadIndex<TypeDefOrRefIndex>();
        }
    }
}

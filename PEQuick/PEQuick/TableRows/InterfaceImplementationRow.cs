using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public struct InterfaceImplementationRow:IRow
    {
        private TypeDefIndex _typeDef;
        private TypeDefOrRefIndex _interface;
        
        public void Read(ref MetaDataReader reader)
        {
            _typeDef = reader.ReadIndex<TypeDefIndex>();
            _interface = reader.ReadIndex<TypeDefOrRefIndex>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class TypeDefRow : Row
    {
        private uint _flags;
        private StringIndex _nameIndex;
        private StringIndex _namespaceIndex;
        private TypeDefOrRefIndex _baseType;
        private FieldIndex _fields;
        private MethodIndex _methods;
        
        public override void Read(ref MetaDataReader reader)
        {
            _flags = reader.Read<uint>();
            _nameIndex = reader.ReadIndex<StringIndex>();
            _namespaceIndex = reader.ReadIndex<StringIndex>();
            _baseType = reader.ReadIndex<TypeDefOrRefIndex>();
            _fields = reader.ReadIndex<FieldIndex>();
            _methods = reader.ReadIndex<MethodIndex>();
        }
    }
}

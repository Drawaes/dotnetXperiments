using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class TypeDefRow
    {
        private uint _flags;
        private StringIndex _name;
        private StringIndex _namespace;
        private TypeDefOrRefIndex _baseType;
        private FieldIndex _fields;
        private MethodIndex _methods;
        public FieldIndex FieldsEnd { get; set; }
        public MethodIndex MethodsEnd { get; set; }
        public FieldIndex Fields => _fields;
        public MethodIndex Methods => _methods;

        public TypeDefRow(ref MetaDataReader reader)
        {
            _flags = reader.Read<uint>();
            _name = reader.ReadIndex<StringIndex>();
            _namespace = reader.ReadIndex<StringIndex>();
            _baseType = reader.ReadIndex<TypeDefOrRefIndex>();
            _fields = reader.ReadIndex<FieldIndex>();
            _methods = reader.ReadIndex<MethodIndex>();
            FieldsEnd = default(FieldIndex);
            MethodsEnd = default(MethodIndex);
        }
    }
}

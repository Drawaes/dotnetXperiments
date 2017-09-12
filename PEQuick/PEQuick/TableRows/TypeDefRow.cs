using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public struct TypeDefRow
    {
        private uint _flags;
        private uint _name;
        private uint _namespace;
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
            _name = reader.ReadStringOffset();
            _namespace = reader.ReadStringOffset();
            _baseType = reader.ReadTypeDefOrRefIndex();
            _fields = new FieldIndex(ref reader);
            _methods = new MethodIndex(ref reader);
            FieldsEnd = default(FieldIndex);
            MethodsEnd = default(MethodIndex);
        }

        public void AddField(FieldRow fieldRow)
        {
            
        }
    }
}

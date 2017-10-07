using System;
using System.Collections.Generic;
using System.Text;
using Vacuum.Core.Clr.Flags;
using Vacuum.Core.Clr.Indexes;

namespace Vacuum.Core.Clr.Rows
{
    public class TypeDef : Row
    {
        private uint _flags;
        private StringIndex _nameIndex;
        private StringIndex _namespaceIndex;
        private TypeDefOrRefIndex _baseType;
        private FieldIndex _firstField;
        private MethodIndex _firstMethod;

        public TypeDef()
        {
        }
        
        public override TableFlag Flag => TableFlag.TypeDef;

        internal override void LoadFromReader(ref ClrMetaReader reader, int index)
        {
            _index = index;
            _flags = reader.Read<uint>();
            _nameIndex = reader.ReadIndex<StringIndex>();
            _namespaceIndex = reader.ReadIndex<StringIndex>();
            _baseType = reader.ReadIndex<TypeDefOrRefIndex>();
            _firstField = reader.ReadIndex<FieldIndex>();
            _firstMethod = reader.ReadIndex<MethodIndex>();
        }

        internal override void Resolve(ClrData clrData)
        {
            throw new NotImplementedException();
        }
    }
}

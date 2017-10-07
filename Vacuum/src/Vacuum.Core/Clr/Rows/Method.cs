using System;
using System.Collections.Generic;
using System.Text;
using Vacuum.Core.Clr.Flags;
using Vacuum.Core.Clr.Indexes;

namespace Vacuum.Core.Clr.Rows
{
    public class Method : Row
    {
        private uint _rva;
        private ushort _methodImplAttributes;
        private MethodAttributesFlags _flags;
        private StringIndex _nameIndex;
        private BlobIndex _signature;
        private ParamIndex _firstParam;

        public Method()
        {
        }
        
        public override TableFlag Flag => TableFlag.Method;

        internal override void LoadFromReader(ref ClrMetaReader reader, int index)
        {
            _index = index;
            _rva = reader.Read<uint>();
            _methodImplAttributes = reader.Read<ushort>();
            _flags = reader.Read<MethodAttributesFlags>();
            _nameIndex = reader.ReadIndex<StringIndex>();
            _signature = reader.ReadIndex<BlobIndex>();
            _firstParam = reader.ReadIndex<ParamIndex>();
        }

        internal override void Resolve(ClrData clrData)
        {
            throw new NotImplementedException();
        }
    }
}

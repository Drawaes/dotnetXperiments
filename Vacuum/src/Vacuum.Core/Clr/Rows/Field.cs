using System;
using System.Collections.Generic;
using System.Text;
using Vacuum.Core.Clr.Flags;
using Vacuum.Core.Clr.Indexes;

namespace Vacuum.Core.Clr.Rows
{
    public class Field : Row
    {
        private ushort _flags;
        private StringIndex _nameIndex;
        private BlobIndex _signature;

        public Field()
        {
        }
        
        public override TableFlag Flag => TableFlag.Field;

        internal override void LoadFromReader(ref ClrMetaReader reader, int index)
        {
            _index = index;
            _flags = reader.Read<ushort>();
            _nameIndex = reader.ReadIndex<StringIndex>();
            _signature = reader.ReadIndex<BlobIndex>();
        }

        internal override void Resolve(ClrData clrData)
        {
            throw new NotImplementedException();
        }
    }
}

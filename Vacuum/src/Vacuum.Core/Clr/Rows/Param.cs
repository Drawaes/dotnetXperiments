using System;
using System.Collections.Generic;
using System.Text;
using Vacuum.Core.Clr.Flags;
using Vacuum.Core.Clr.Indexes;

namespace Vacuum.Core.Clr.Rows
{
    public class Param : Row
    {
        private ushort _flags;
        private ushort _sequence;
        private StringIndex _nameIndex;

        public Param()
        {
        }
        
        public override TableFlag Flag => TableFlag.Param;

        internal override void LoadFromReader(ref ClrMetaReader reader, int index)
        {
            _index = index;
            _flags = reader.Read<ushort>();
            _sequence = reader.Read<ushort>();
            _nameIndex = reader.ReadIndex<StringIndex>();
        }

        internal override void Resolve(ClrData clrData)
        {
            throw new NotImplementedException();
        }
    }
}

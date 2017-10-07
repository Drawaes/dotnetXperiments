using System;
using System.Collections.Generic;
using System.Text;
using Vacuum.Core.Clr.Flags;
using Vacuum.Core.Clr.Indexes;

namespace Vacuum.Core.Clr.Rows
{
    public class MemberRef : Row
    {
        private MemberRefParentIndex _class;
        private StringIndex _nameIndex;
        private BlobIndex _signature;

        public MemberRef()
        {
        }

        public override TableFlag Flag => TableFlag.MemberRef;

        internal override void LoadFromReader(ref ClrMetaReader reader, int index)
        {
            _index = index;
            _class = reader.ReadIndex<MemberRefParentIndex>();
            _nameIndex = reader.ReadIndex<StringIndex>();
            _signature = reader.ReadIndex<BlobIndex>();
        }

        internal override void Resolve(ClrData clrData)
        {
            throw new NotImplementedException();
        }
    }
}

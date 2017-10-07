using System;
using System.Collections.Generic;
using System.Text;
using Vacuum.Core.Clr.Flags;
using Vacuum.Core.Clr.Indexes;

namespace Vacuum.Core.Clr.Rows
{
    public class CustomAttribute : Row
    {
        private HasCustomAttributeIndex _parent;
        private CustomAttributeTypeIndex _type;
        private BlobIndex _value;

        public CustomAttribute()
        {
        }
        
        public override TableFlag Flag => TableFlag.CustomAttribute;

        internal override void LoadFromReader(ref ClrMetaReader reader, int index)
        {
            _index = index;
            _parent = reader.ReadIndex<HasCustomAttributeIndex>();
            _type = reader.ReadIndex<CustomAttributeTypeIndex>();
            _value = reader.ReadIndex<BlobIndex>();
        }

        internal override void Resolve(ClrData clrData)
        {
            throw new NotImplementedException();
        }
    }
}

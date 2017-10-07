using System;
using System.Collections.Generic;
using System.Text;
using Vacuum.Core.Clr.Flags;

namespace Vacuum.Core.Clr.Rows
{
    public class PropertyMap : Row
    {
        public PropertyMap()
        {
        }
        
        public override TableFlag Flag => TableFlag.PropertyMap;

        internal override void LoadFromReader(ref ClrMetaReader reader, int index)
        {
            throw new NotImplementedException();
        }

        internal override void Resolve(ClrData clrData)
        {
            throw new NotImplementedException();
        }
    }
}

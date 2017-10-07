using System;
using System.Collections.Generic;
using System.Text;
using Vacuum.Core.Clr.Flags;

namespace Vacuum.Core.Clr.Rows
{
    public class ManifestResource : Row
    {
        public ManifestResource()
        {
        }

        public override TableFlag Flag => TableFlag.ManifestResource;

        internal override void LoadFromReader(ref ClrMetaReader reader, int index) => throw new NotImplementedException();

        internal override void Resolve(ClrData clrData)
        {
            throw new NotImplementedException();
        }
    }
}

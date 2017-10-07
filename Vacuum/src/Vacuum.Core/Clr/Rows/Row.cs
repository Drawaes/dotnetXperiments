using System;
using System.Collections.Generic;
using System.Text;
using Vacuum.Core.Clr.Flags;
using Vacuum.Core.Clr.Indexes;

namespace Vacuum.Core.Clr.Rows
{
    public abstract class Row
    {
        protected int _index;

        public Row() { }

        public abstract TableFlag Flag { get; }
        public Token Token => new Token(Flag, _index);

        internal abstract void LoadFromReader(ref ClrMetaReader reader, int index);
        internal abstract void Resolve(ClrData clrData);
    }
}

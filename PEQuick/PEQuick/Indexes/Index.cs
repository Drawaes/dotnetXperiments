using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;

namespace PEQuick.Indexes
{
    public abstract class Index
    {
        internal abstract Span<byte> Write(Span<byte> input, Dictionary<uint,uint> remapper, bool largeFormat);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace PEQuick
{
    public interface IMemorySize
    {
        long StackReserveSize { get; set; }
        long StackCommitSize { get; set; }
        long HeapReserveSize { get; set; }
        long HeapCommitSize { get; set; }
    }
}

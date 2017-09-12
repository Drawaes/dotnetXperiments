using System;
using System.Collections.Generic;
using System.Text;

namespace PEQuick
{
    public interface IMemorySize
    {
        long StackReserveSize { get; }
        long StackCommitSize { get; }
        long HeapReserveSize { get; }
        long HeapCommitSize { get; }
        uint LoaderFlags { get; }
        uint NumberOfRvaAndSizes { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace PEQuick
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MemorySize64 : IMemorySize
    {
        private long _stackReserveSize;
        private long _stackCommitSize;
        private long _heapReserveSize;
        private long _heapCommitSize;
        private uint _loaderFlags;
        private uint _numberOfDataDir;

        public long StackReserveSize => _stackReserveSize;
        public long StackCommitSize => _stackCommitSize;
        public long HeapReserveSize => _heapReserveSize;
        public long HeapCommitSize => _heapCommitSize;
        public uint LoaderFlags => _loaderFlags;
        public uint NumberOfRvaAndSizes => _numberOfDataDir;
    }
}

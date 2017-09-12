using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace PEQuick
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MemorySize32 : IMemorySize
    {
        private uint _stackReserveSize;
        private uint _stackCommitSize;
        private uint _heapReserveSize;
        private uint _heapCommitSize;
        private uint _loaderFlags;
        private uint _numberOfRvaAndSizes;

        public long StackReserveSize => _stackReserveSize;
        public long StackCommitSize => _stackCommitSize;
        public long HeapReserveSize => _heapReserveSize;
        public long HeapCommitSize => _heapCommitSize;
        public uint LoaderFlags => _loaderFlags;
        public uint NumberOfRvaAndSizes => _numberOfRvaAndSizes;
    }
}

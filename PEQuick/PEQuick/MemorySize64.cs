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
       
        public long StackReserveSize { get => _stackReserveSize; set => _stackReserveSize = value; }
        public long StackCommitSize { get => _stackCommitSize; set => _stackCommitSize = value; }
        public long HeapReserveSize { get => _heapReserveSize; set => _heapReserveSize = value; }
        public long HeapCommitSize { get => _heapCommitSize; set => _heapCommitSize = value; }

        internal static MemorySize64 CreateDefaults()
        {
            var mem = new MemorySize64()
            {
                StackReserveSize = 0x100000,
                StackCommitSize = 0x1000,
                HeapReserveSize = 0x100000,
                HeapCommitSize = 0x1000,
            };
            return mem;
        }
    }
}

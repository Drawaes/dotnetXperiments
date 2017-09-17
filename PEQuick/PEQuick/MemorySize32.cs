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

        public long StackReserveSize { get => _stackReserveSize; set => _stackReserveSize = (uint)value; }
        public long StackCommitSize { get => _stackCommitSize; set => _stackCommitSize = (uint)value; }
        public long HeapReserveSize { get => _heapReserveSize; set => _heapReserveSize = (uint)value; }
        public long HeapCommitSize { get => _heapCommitSize; set => _heapCommitSize = (uint)value; }

        internal static MemorySize32 CreateDefaults()
        {
            var mem = new MemorySize32()
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

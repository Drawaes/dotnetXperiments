using System;
using System.Collections.Generic;
using System.Text;

namespace Vacuum.Core.PE
{
    public class MemorySizes
    {
        private long _stackReserveSize;
        private long _stackCommitSize;
        private long _heapReserveSize;
        private long _heapCommitSize;

        public MemorySizes(ref ReadOnlySpan<byte> buffer, bool is64)
        {
            if (is64)
            {
                buffer = buffer.Read(out _stackReserveSize);
                buffer = buffer.Read(out _stackCommitSize);
                buffer = buffer.Read(out _heapReserveSize);
                buffer = buffer.Read(out _heapCommitSize);
            }
            else
            {
                buffer = buffer.Read(out int value);
                _stackReserveSize = value;
                buffer = buffer.Read(out value);
                _stackCommitSize = value;
                buffer = buffer.Read(out value);
                _heapReserveSize = value;
                buffer = buffer.Read(out value);
                _heapCommitSize = value;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;

namespace PEQuick
{
    public class BlobSection
    {
        private byte[] _section;

        public BlobSection(Span<byte> input)
        {
            _section = input.ToArray();
        }

        public byte[] GetBlob(uint blobIndex)
        {
            var span = _section.AsSpan().Slice((int)blobIndex);
            span = span.ReadEncodedInt(out uint size);
            return span.Slice(0, (int)size).ToArray();
        }
    }
}

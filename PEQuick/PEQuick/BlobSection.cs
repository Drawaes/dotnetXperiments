using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;

namespace PEQuick
{
    public class BlobSection
    {
        private byte[] _section;
        private Dictionary<uint, byte[]> _blobs = new Dictionary<uint, byte[]>();

        public BlobSection(Span<byte> input)
        {
            _section = input.ToArray();
        }

        public byte[] GetBlob(uint blobIndex)
        {
            if(_blobs.TryGetValue(blobIndex, out byte[] blob))
            {
                return blob;
            }
            var span = _section.AsSpan().Slice((int)blobIndex);
            span = span.ReadEncodedInt(out uint size);
            blob = span.Slice(0, (int)size).ToArray();
            _blobs.Add(blobIndex, blob);
            return blob;
        }
    }
}

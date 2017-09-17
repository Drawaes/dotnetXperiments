using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PEQuick.Flags;
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

        public Span<byte> WriteSection(Dictionary<uint,uint> remapper)
        {
            var tableTag = ((uint)TableFlag.Blob << 24);

            var buffer = new byte[_blobs.Sum(b => b.Value.Length) + (3 * _blobs.Count)];
            var remappedTags = new Dictionary<uint, uint>();

            var span = new Span<byte>(buffer);
            span.Write((byte)0);
            span = span.Slice(1);

            //Now we need to write each of the blobs, with a length prefix
            foreach(var kv in _blobs)
            {
                var index = (uint)(((uint)buffer.Length - span.Length) | tableTag);
                span = span.WriteEncodedInt((uint)kv.Value.Length);
                kv.Value.CopyTo(span);
                span = span.Slice(kv.Value.Length);
                remappedTags.Add(kv.Key | tableTag, index);
            }
            var totalSize = buffer.Length - span.Length;
            return buffer.AsSpan().Slice(0, totalSize);
        }
    }
}

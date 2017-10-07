using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;

namespace PEQuick
{
    public class GuidSection
    {
        private Dictionary<int, Guid> _guids = new Dictionary<int, Guid>();

        public GuidSection(Span<byte> input)
        {
            var initialSize = input.Length;
            while (input.Length > 0)
            {
                var currentIndex = initialSize - input.Length;
                var guid = new Guid(input.Slice(0, 16).ToArray());
                input = input.Slice(16);
                _guids.Add(currentIndex, guid);
            }
        }

        public Guid GetGuid(uint index)
        {
            throw new NotImplementedException();
        }

        internal Span<byte> WriteSection(Dictionary<uint, uint> remapper)
        {
            var tag = ((uint)TableFlag.Guid << 24);
            var maxSize = _guids.Values.Count * 16 + 1;
            var buffer = new byte[maxSize];
            var span = new Span<byte>(buffer);
            span[0] = 0;
            span = span.Slice(1);

            foreach (var kv in _guids)
            {
                var index = (uint)(buffer.Length - span.Length) | tag;
                span = span.Write(kv.Value);
                remapper.Add((uint)kv.Key | tag, index);
            }
            return buffer.AsSpan().Slice(0, buffer.Length - span.Length);
        }
    }
}

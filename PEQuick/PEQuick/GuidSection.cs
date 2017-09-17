using System;
using System.Collections.Generic;
using System.Text;

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
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;

namespace PEQuick
{
    public class StringsSection
    {
        private Dictionary<int, string> _strings = new Dictionary<int, string>();
        private byte[] _section;

        public StringsSection(Span<byte> input)
        {
            _section = input.ToArray();
        }

        public string GetString(StringIndex stringIndex)
        {
            var span = _section.AsSpan().Slice(stringIndex.Index);
            var nextNull = span.IndexOf(0);
            return span.Slice(0,nextNull).ReadNullString();
        }
    }
}

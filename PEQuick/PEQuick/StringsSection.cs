using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PEQuick.Indexes;

namespace PEQuick
{
    public class StringsSection
    {
        private Dictionary<uint, string> _strings = new Dictionary<uint, string>();
        private byte[] _section;

        public StringsSection(Span<byte> input)
        {
            _section = input.ToArray();
        }

        public string GetString(uint stringIndex)
        {
            var span = _section.AsSpan().Slice((int)stringIndex);
            var nextNull = span.IndexOf(0);
            var s = span.Slice(0,nextNull).ReadNullString();
            if(_strings.TryGetValue(stringIndex, out string oldValue))
            {
                if(oldValue != s)
                {
                    throw new NotImplementedException();
                }
                return oldValue;
            }
            _strings.Add(stringIndex, s);
            return s;
        }

        public void MergeDuplicates()
        {
            foreach(var kv in _strings.GroupBy(kv => kv.Value))
            {
                if(kv.Count() > 1)
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}

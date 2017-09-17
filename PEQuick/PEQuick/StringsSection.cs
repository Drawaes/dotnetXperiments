using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PEQuick.Flags;
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

        internal Span<byte> WriteSection(Dictionary<uint, uint> remapper)
        {
            var tag = ((uint)TableFlag.Strings << 24);
            var maxSize = _strings.Sum(s => s.Value.Length + 4) + 1;
            var buffer = new byte[maxSize];
            var span = new Span<byte>(buffer);
            span[0] = 0;
            span = span.Slice(1);
            
            foreach(var kv in _strings)
            {
                var index = (uint)(buffer.Length - span.Length) | tag;
                span = span.WriteEncodedInt((uint)kv.Value.Length + 1);
                var charSpan = new Span<char>(kv.Value.ToCharArray()).AsBytes();
                System.Text.Encoders.Utf8.FromUtf16(charSpan, span, out int consumed, out int written);
                span = span.Slice(written);
                span[0] = 0;
                span = span.Slice(1);
                remapper.Add(kv.Key | tag, index);
            }
            return buffer.AsSpan().Slice(0, buffer.Length - span.Length);
        }
    }
}

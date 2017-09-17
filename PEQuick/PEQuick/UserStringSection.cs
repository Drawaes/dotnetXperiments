using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PEQuick.Flags;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick
{
    public class UserStringSection :ITable
    {
        private Dictionary<int, TableRows.UserStringRow> _strings = new Dictionary<int, TableRows.UserStringRow>();
        
        public UserStringSection(Span<byte> input)
        {
            var originalSize = input.Length;
            input = input.Slice(1);
            var counter = 1;
            while(input.Length > 0)
            {
                counter = originalSize - input.Length;
                input = input.ReadEncodedInt(out uint length);
                string s;
                byte token;
                if (length == 0)
                {
                    s = null;
                    token = 0;
                }
                else if (length == 1)
                {
                    s = string.Empty;
                    token = input[0];
                    input = input.Slice(1);
                }
                else
                {
                    var i = input.Slice(0, (int)length - 1);
                    s = Encoding.Unicode.GetString(i.ToArray());
                    token = i[i.Length - 1];
                    input = input.Slice((int)length);
                }
                
                var cString = new UserStringRow((uint)counter, s, token);
                _strings.Add(counter, cString);
                
            }
        }

        public TableFlag TableFlag => throw new NotImplementedException();

        public int Count => throw new NotImplementedException();

        public void AddRow(Row newRow)
        {
            _strings.Add(_strings.Max(kv => kv.Key) + 1, (UserStringRow)newRow);
        }

        public IEnumerator<Row> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public Row GetRow(int index)
        {
            if (_strings.TryGetValue(index, out UserStringRow row))
            {
                return row;
            }
            return _strings.Values.First();
        }

        public void LoadFromMemory(ref MetaDataReader reader, int size)
        {
            throw new NotImplementedException();
        }

        internal Span<byte> WriteSection(Dictionary<uint, uint> remapper)
        {
            var tag = ((uint)TableFlag.UserString << 24);
            var maxSize = _strings.Sum(s => (s.Value?.Value?.Length ?? 0 ) * 2 + 4) + 1;
            var buffer = new byte[maxSize];
            var span = new Span<byte>(buffer);
            span[0] = 0;
            span = span.Slice(1);

            foreach (var kv in _strings)
            {
                var index = (uint)(buffer.Length - span.Length) | tag;
                if (kv.Value.Value == null)
                {
                    //No string so just write a zero length
                    span = span.WriteEncodedInt(0);
                }
                else if (kv.Value.Value == string.Empty)
                {
                    span = span.WriteEncodedInt(1);
                    span[0] = kv.Value.Token;
                    span = span.Slice(1);
                }
                else
                {
                    var charSpan = new Span<char>(kv.Value.Value.ToCharArray()).AsBytes();
                    span = span.WriteEncodedInt((uint)charSpan.Length + 1);
                    charSpan.CopyTo(span);
                    span = span.Slice(charSpan.Length);
                    span[0] = kv.Value.Token;
                    span = span.Slice(1);
                }
                remapper.Add((uint)kv.Key | tag, index);
            }
            return buffer.AsSpan().Slice(0, buffer.Length - span.Length);
        }

        internal void MergeDuplicates()
        {
            foreach(var kv in _strings.GroupBy(kv => kv.Value))
            {
                if(kv.Count() > 1)
                {
                    throw new NotImplementedException();
                }
            }
        }

        public void Resolve(MetaDataTables metaDataTables)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        
        public void Write(ref MetaDataWriter writer, Dictionary<uint, uint> remapper)
        {
            throw new NotImplementedException();
        }
    }
}

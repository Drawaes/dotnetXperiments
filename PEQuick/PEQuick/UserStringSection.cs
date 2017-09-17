using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PEQuick.TableRows;

namespace PEQuick
{
    public class UserStringSection
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

        public UserStringRow GetRow(uint index)
        {
            if (_strings.TryGetValue((int)index, out UserStringRow row))
            {
                return row;
            }
            return _strings.Values.First();
        }
    }
}

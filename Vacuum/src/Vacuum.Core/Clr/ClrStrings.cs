using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vacuum.Core.Clr.Indexes;

namespace Vacuum.Core.Clr
{
    public class ClrStrings
    {
        private Dictionary<Token, string> _strings = new Dictionary<Token, string>();
        private int _currentLength;
        private byte[] _temp;

        public ClrStrings(ClrReader reader)
        {
            _temp = reader.CurrentSpan.ToArray();
            _currentLength = reader.Remaining;
            //throw away byte
            var ignore = reader.Read<byte>();
            while (reader.Remaining > 0)
            {
                var token = new Token(Flags.TableFlag.Strings, reader.CurrentIndex);
                _strings.Add(token, reader.ReadNullTerminatedAscii());
            }
        }

        internal string GetString(uint rawIndex)
        {
            var token = new Token(Flags.TableFlag.Strings, (int)rawIndex & 0x00FFFFFF);
            if (token.Index == 0)
            {
                return null;
            }
            if(_strings.TryGetValue(token, out string value))
            {
                return value;
            }
            var newREader = new ClrReader(_temp);
            newREader.Advance((int)rawIndex);
            value = newREader.ReadNullTerminatedAscii();
            _strings.Add(token, value);
            return value;
        }
    }
}

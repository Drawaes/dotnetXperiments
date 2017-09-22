using System;
using System.Collections.Generic;
using System.Text;
using Vacuum.Core.Clr.Indexes;

namespace Vacuum.Core.Clr
{
    public class ClrStrings
    {
        private Dictionary<Token, string> _strings = new Dictionary<Token, string>();
        private int _currentLength;

        public ClrStrings(ClrReader reader)
        {
            _currentLength = reader.Remaining;
            //throw away byte
            var ignore = reader.Read<byte>();
            while(reader.Remaining > 0)
            {
                var token = new Token(Flags.TableFlag.Strings, reader.CurrentIndex);
                _strings.Add(token, reader.ReadNullTerminatedAscii());
            }
        }
    }
}

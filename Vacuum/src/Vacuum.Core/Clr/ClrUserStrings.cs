using System;
using System.Collections.Generic;
using System.Text;
using Vacuum.Core.Clr.Indexes;

namespace Vacuum.Core.Clr
{
    public class ClrUserStrings
    {
        private Dictionary<Token, string> _tokens = new Dictionary<Token, string>();
        private int _currentLength;

        public ClrUserStrings(ClrReader reader)
        {
            _currentLength = reader.Remaining;
            var ignore = reader.Read<byte>();

            while(reader.Remaining > 0)
            {
                var token = new Token(Flags.TableFlag.UserString, reader.CurrentIndex);
                var length = reader.ReadEncodedInt();
                var str = reader.ReadFixedLengthUtf16((int)length);
                _tokens.Add(token, str);
            }
        }
    }
}

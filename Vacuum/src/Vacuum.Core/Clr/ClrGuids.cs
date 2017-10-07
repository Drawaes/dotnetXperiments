using System;
using System.Collections.Generic;
using System.Text;
using Vacuum.Core.Clr.Indexes;

namespace Vacuum.Core.Clr
{
    public class ClrGuids
    {
        private Dictionary<Token, Guid> _guids = new Dictionary<Token, Guid>();
        private int _currentLength;

        public ClrGuids(ClrReader reader)
        {
            _currentLength = reader.Remaining;
            var currentIndex = 0;
            while(reader.Remaining > 0)
            {
                currentIndex++;
                var token = new Token(Flags.TableFlag.Guid, currentIndex);
                _guids.Add(token, reader.Read<Guid>());
            }
        }

        internal Guid? GetGuid(uint rawIndex)
        {
            var token = new Token(Flags.TableFlag.Strings, (int)rawIndex & 0x00FFFFFF);
            if (token.Index == 0)
            {
                return null;
            }
            return _guids[token];
        }
    }
}

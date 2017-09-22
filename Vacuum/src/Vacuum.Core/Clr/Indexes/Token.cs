using System;
using System.Collections.Generic;
using System.Text;
using Vacuum.Core.Clr.Flags;

namespace Vacuum.Core.Clr.Indexes
{
    public struct Token
    {
        private uint _token;
        public TableFlag Table => (TableFlag)(_token >> 24);
        public int Index => (int)(_token & 0x00FF_FFFF);

        public Token(TableFlag flag, int index) => _token = ((uint)flag << 24) | (uint)index;
        public Token(uint token) => _token = token;
    }
}

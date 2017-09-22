using System;
using System.Collections.Generic;
using System.Text;

namespace Vacuum.Core.Utils
{
    public class Utils
    {
        public static uint Align(uint value, uint alignment)
        {
            alignment--;
            value = (value + alignment) & ~alignment;
            return value;
        }
    }
}

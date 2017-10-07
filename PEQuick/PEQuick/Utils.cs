using System;
using System.Collections.Generic;
using System.Text;

namespace PEQuick
{
    public class Utils
    {
        public static uint Align(uint size, uint alignment)
        {
            alignment--;
            return (size + alignment) & ~alignment;
        }
    }
}

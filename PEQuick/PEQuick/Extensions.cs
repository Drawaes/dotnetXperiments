using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;

namespace PEQuick
{
    public static class Extensions
    {
        public static int GetSize(this Dictionary<MetadataTableFlags, int> self, MetadataTableFlags flag)
        {
            if(self.TryGetValue(flag, out int value))
            {
                return value;
            }
            return 0;
        }
    }
}

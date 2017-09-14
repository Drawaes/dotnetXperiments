using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class EventMapTable
    {
        public EventMapTable(MetaDataTables tables, ref MetaDataReader reader)
        {
            if(tables.Sizes.GetSize(MetadataTableFlags.EventMap) > 0)
            {
                throw new NotImplementedException();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class EventTable
    {
        public EventTable(MetaDataTables tables, ref MetaDataReader reader)
        {
            if (tables.Sizes.GetSize(MetadataTableFlags.Event) > 0)
            {
                throw new NotImplementedException();
            }
        }
    }
}

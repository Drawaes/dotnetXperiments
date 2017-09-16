using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class EventIndex : Index
    {
        private EventRow _row;

        public int Index => checked((int)_rawIndex);

        internal override void Resolve(MetaDataTables tables)
        {
            _row = tables.GetCollection<EventRow>()[Index];
        }
    }
}

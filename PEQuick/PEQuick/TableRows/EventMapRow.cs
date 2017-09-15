using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class EventMapRow : Row
    {
        private TypeDefIndex _parent;
        private EventIndex _eventList;

        public override void Read(ref MetaDataReader reader)
        {
            _parent = reader.ReadIndex<TypeDefIndex>();
            _eventList = reader.ReadIndex<EventIndex>();
        }
    }
}

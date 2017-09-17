using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class EventMapRow : Row
    {
        private TypeDefIndex _parent;
        private EventIndex _firstEvent;
        private EventRow[] _events;

        public override TableFlag Table => TableFlag.EventMap;
        public override uint AssemblyTag => _parent.Row.AssemblyTag;

        public override void Resolve(MetaDataTables tables)
        {
            _parent.Resolve(tables);
            _firstEvent.Resolve(tables);

            var nextEventMap = tables.GetCollection<EventMapRow>()[Index + 1];
            _events = tables.GetCollection<EventRow>().GetRange(_firstEvent.Index, nextEventMap?._firstEvent?.Index ?? int.MaxValue);
            foreach (var e in _events)
            {
                e.Parent = this;
            }
        }

        public override void Read(ref MetaDataReader reader)
        {
            _parent = reader.ReadIndex<TypeDefIndex>();
            _firstEvent = reader.ReadIndex<EventIndex>();
        }

        public override void WriteRow(ref MetaDataWriter writer, Dictionary<uint, uint> tokenRemapping)
        {
            throw new NotImplementedException();
        }
    }
}

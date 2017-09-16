using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class EventRow : Row
    {
        private StringIndex _nameIndex;

        public EventAttrFlags Flags { get; set; }
        public TypeDefOrRefIndex EventType { get; set; }
        public override TableFlag Table => TableFlag.Event;
        public override uint AssemblyTag => Parent.AssemblyTag;

        public EventMapRow Parent { get; internal set; }

        public override void Resolve(MetaDataTables tables)
        {
            _nameIndex.Resolve(tables);
            EventType.Resolve(tables);
        }

        public override void Read(ref MetaDataReader reader)
        {
            Flags = reader.Read<EventAttrFlags>();
            _nameIndex = reader.ReadIndex<StringIndex>();
            EventType = reader.ReadIndex<TypeDefOrRefIndex>();
        }
    }
}

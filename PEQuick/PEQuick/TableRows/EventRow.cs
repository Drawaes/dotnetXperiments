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
        public EventAttrFlags Flags { get; set; }
        private StringIndex _nameIndex;
        public TypeDefOrRefIndex EventType { get; set; }
        
        public override void Read(ref MetaDataReader reader)
        {
            Flags = reader.Read<EventAttrFlags>();
            _nameIndex = reader.ReadIndex<StringIndex>();
            EventType = reader.ReadIndex<TypeDefOrRefIndex>();
        }
    }
}

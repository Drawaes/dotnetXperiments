using System;
using System.Collections.Generic;
using System.Text;
using Vacuum.Core.Clr.Flags;
using Vacuum.Core.Clr.Indexes;

namespace Vacuum.Core.Clr.Rows
{
    public class Module : Row
    {
        private ushort _generation;
        private StringIndex _name;
        private GuidIndex _mvid;
        private GuidIndex _encId;
        private GuidIndex _encBaseId;
        private Assembly _parent;

        public Module() : base()
        {
        }

        public override TableFlag Flag => TableFlag.Module;

        internal override void LoadFromReader(ref ClrMetaReader reader, int index)
        {
            _index = index;
            _generation = reader.Read<ushort>();
            _name = reader.ReadIndex<StringIndex>();
            _mvid = reader.ReadIndex<GuidIndex>();
            _encId = reader.ReadIndex<GuidIndex>();
            _encBaseId = reader.ReadIndex<GuidIndex>();            
        }

        internal override void Resolve(ClrData clrData)
        {
            _parent = clrData.GetTable<Assembly>()[1];
            _name.Resolve(clrData);
            _mvid.Resolve(clrData);
            _encId.Resolve(clrData);
            _encBaseId.Resolve(clrData);
        }
    }
}

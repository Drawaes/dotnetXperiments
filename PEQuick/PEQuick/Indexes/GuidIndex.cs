using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class GuidIndex : SimpleIndex
    {
        private Guid _value;

        public int Index => (int)_rawIndex;
        public Guid Value => _value;
        public override uint TableOffset => (uint)TableFlag.Guid << 24;

        internal override void Resolve(MetaDataTables tables)
        {
            _value = tables.Guids.GetGuid(_rawIndex);
        }
    }
}

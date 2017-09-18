using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class FieldIndex : SingleIndex
    {
        private FieldRow _field;

        public int Index => checked((int)_rawIndex);
        public override Row Row => _field;

        internal override void Resolve(MetaDataTables tables)
        {
            _field = tables.GetCollection<FieldRow>()[Index];
        }
    }
}

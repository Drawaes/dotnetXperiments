using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class PropertyIndex : Index
    {
        private Row _row;

        public int Index => checked((int)_rawIndex);

        internal override void Resolve(MetaDataTables tables)
        {
            _row = tables.GetCollection<PropertyRow>()[Index];
        }
    }
}

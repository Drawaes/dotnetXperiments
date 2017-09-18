using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class MethodIndex : SingleIndex
    {
        private MethodRow _method;

        public int Index => checked((int)_rawIndex);
        public override Row Row => _method;

        internal override void Resolve(MetaDataTables tables)
        {
            _method = tables.GetCollection<MethodRow>()[Index];
        }
    }
}

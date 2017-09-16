using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class ParamIndex : Index
    {
        private ParamRow _param;
        public int Index => checked((int)_rawIndex);

        internal override void Resolve(MetaDataTables tables)
        {
            _param = tables.GetCollection<ParamRow>()[Index];
        }
    }
}

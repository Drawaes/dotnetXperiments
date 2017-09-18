using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class ParamIndex : SingleIndex
    {
        private ParamRow _param;
        public int Index => checked((int)_rawIndex);

        public override Row Row => _param;

        //public override uint TableOffset => (uint)TableFlag.Param << 24;
        //public override Row Row => _param;

        internal override void Resolve(MetaDataTables tables)
        {
            _param = tables.GetCollection<ParamRow>()[Index];
        }
    }
}

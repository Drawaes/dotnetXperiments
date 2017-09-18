using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class TypeDefIndex : SingleIndex
    {
        private TypeDefRow _row;

        public int Index => checked((int)_rawIndex);
        public override Row Row => _row;

        internal override void Resolve(MetaDataTables tables)
        {
            _row = tables.GetCollection<TypeDefRow>()[Index];
        }
    }
}

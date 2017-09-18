using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class ResolutionScopeIndex : MultiIndex
    {
        protected override byte BitMask => 0b0000_0011;
        protected override byte BitShift => 2;
        private Row _row;

        public override Row Row => _row;

        internal override void Resolve(MetaDataTables tables)
        {
            var flag = _rawIndex & BitMask;
            var index =(int)( _rawIndex >> BitShift);
            switch (flag)
            {
                case 0:
                    _row = tables.GetCollection<ModuleRow>()[index];
                    return;
                case 1:
                    _row = tables.GetCollection<ModuleRefRow>()[index];
                    return;
                case 2:
                    _row = tables.GetCollection<AssemblyRefRow>()[index];
                    return;
                case 3:
                    _row = tables.GetCollection<TypeRefRow>()[index];
                    return;
            }
        }
    }
}

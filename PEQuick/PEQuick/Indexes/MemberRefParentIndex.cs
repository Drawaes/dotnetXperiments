using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class MemberRefParentIndex : MultiIndex
    {
        private Row _row;

        public override Row Row => _row;
        protected override byte BitShift => 3;
        protected override byte BitMask => 0b0000_0111;

        internal override void Resolve(MetaDataTables tables)
        {
            var flag = _rawIndex & BitMask;
            var index = (int)(_rawIndex >> BitShift);
            switch (flag)
            {
                case 0:
                    _row = tables.GetCollection<TypeDefRow>()[index];
                    break;
                case 1:
                    _row = tables.GetCollection<TypeRefRow>()[index];
                    break;
                case 2:
                    _row = tables.GetCollection<ModuleRefRow>()[index];
                    break;
                case 3:
                    _row = tables.GetCollection<MethodRow>()[index];
                    break;
                case 4:
                    _row = tables.GetCollection<TypeSpecRow>()[index];
                    break;
            }
        }
    }
}

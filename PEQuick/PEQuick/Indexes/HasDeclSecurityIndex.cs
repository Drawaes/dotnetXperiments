using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class HasDeclSecurityIndex : MultiIndex
    {
        private Row _row;
        protected override byte BitMask => 0b0000_0011;
        protected override byte BitShift => 2;

        public override Row Row => _row;
        
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
                    _row = tables.GetCollection<MethodRow>()[index];
                    break;
                case 2:
                    _row = tables.GetCollection<AssemblyRow>()[index];
                    break;
            }
        }
    }
}

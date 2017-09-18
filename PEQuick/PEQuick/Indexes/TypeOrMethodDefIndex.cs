﻿using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class TypeOrMethodDefIndex : MultiIndex
    {
        private Row _row;

        public override Row Row => _row;
                
        protected override byte BitMask => 0b0000_0001;
        protected override byte BitShift => 1;

        internal override void Resolve(MetaDataTables tables)
        {
            if (_rawIndex == 0)
            {
                return;
            }
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
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class ImplementationIndex : MultiIndex
    {
        private Row _row;

        protected override byte BitMask => 0b0000_0011;
        protected override byte BitShift => 2;
        public override Row Row => _row;

        internal override void Resolve(MetaDataTables tables)
        {
            if (_rawIndex == 0)
            {
                return;
            }
            var flags = _rawIndex & BitMask;
            var index = (int)(_rawIndex >> BitShift);
            switch (flags)
            {
                case 0:
                    //File
                    throw new NotImplementedException();
                case 1:
                    _row = tables.GetCollection<AssemblyRefRow>()[index];
                    break;
                case 2:
                    //exported type
                    throw new NotImplementedException();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class ImplementationIndex : Index
    {
        private Row _row;
        private const uint BitMask = 0b0000_0011;

        internal override void Resolve(MetaDataTables tables)
        {
            if (_rawIndex == 0)
            {
                return;
            }
            var flags = _rawIndex & BitMask;
            var index = (int)(_rawIndex >> 2);
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

        internal override Span<byte> Write(Span<byte> input, Dictionary<uint, uint> remapper, bool largeFormat)
        {
            throw new NotImplementedException();
        }
    }
}

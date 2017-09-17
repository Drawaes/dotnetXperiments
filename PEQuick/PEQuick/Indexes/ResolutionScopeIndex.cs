using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class ResolutionScopeIndex : Index
    {
        private const uint BitMask = 0b0000_0011;
        private Row _row;

        public Row Row => _row;

        internal override void Resolve(MetaDataTables tables)
        {
            var flag = _rawIndex & BitMask;
            var index =(int)( _rawIndex >> 2);
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

        internal override Span<byte> Write(Span<byte> input, Dictionary<uint, uint> remapper, bool largeFormat)
        {
            throw new NotImplementedException();
        }
    }
}

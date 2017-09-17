using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class MethodIndex : Index
    {
        private MethodRow _method;

        public int Index => checked((int)_rawIndex);
        public MethodRow Row => _method;

        internal override void Resolve(MetaDataTables tables)
        {
            _method = tables.GetCollection<MethodRow>()[Index];
        }

        internal override Span<byte> Write(Span<byte> input, Dictionary<uint, uint> remapper, bool largeFormat)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class FieldIndex : Index
    {
        private FieldRow _field;

        public int Index => checked((int)_rawIndex);
        public FieldRow Row => _field;

        internal override void Resolve(MetaDataTables tables)
        {
            _field = tables.GetCollection<FieldRow>()[Index];
        }

        internal override Span<byte> Write(Span<byte> input, Dictionary<uint, uint> remapper, bool largeFormat)
        {
            throw new NotImplementedException();
        }
    }
}

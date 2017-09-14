using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public struct FieldLayoutRow : IRow
    {
        private uint _offset;
        private FieldIndex _field;

        public void Read(ref MetaDataReader reader)
        {
            _offset = reader.Read<uint>();
            _field = reader.ReadIndex<FieldIndex>();
        }
    }
}

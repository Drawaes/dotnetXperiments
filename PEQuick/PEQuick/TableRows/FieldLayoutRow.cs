using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class FieldLayoutRow : Row
    {
        private uint _offset;
        private FieldIndex _field;

        public override void Read(ref MetaDataReader reader)
        {
            _offset = reader.Read<uint>();
            _field = reader.ReadIndex<FieldIndex>();
        }
    }
}

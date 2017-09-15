using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class FieldRVA : Row
    {
        public uint RVA;
        public FieldIndex Field;

        public override void Read(ref MetaDataReader reader)
        {
            RVA = reader.Read<uint>();
            Field = reader.ReadIndex<FieldIndex>();
        }
    }
}

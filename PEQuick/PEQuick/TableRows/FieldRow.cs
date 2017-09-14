﻿using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class FieldRow
    {
        private ushort _flags;
        private StringIndex _name;
        private BlobIndex _signature;

        public FieldRow(ref MetaDataReader reader)
        {
            _flags = reader.Read<ushort>();
            _name = reader.ReadIndex<StringIndex>();
            _signature = reader.ReadIndex<BlobIndex>();
        }
    }
}

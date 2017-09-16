﻿using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public interface ITable : IEnumerable<Row>
    {
        TableFlag TableFlag { get; }
        void LoadFromMemory(ref MetaDataReader reader, int size);
        void Resolve(MetaDataTables metaDataTables);
    }
}

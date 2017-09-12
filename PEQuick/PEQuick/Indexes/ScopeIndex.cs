﻿using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;

namespace PEQuick.Indexes
{
    public struct ScopeIndex
    {
        private uint _scopeIndex;

        public ScopeIndex(ref MetaDataReader reader) => _scopeIndex = reader.Read<ushort>();
    }
}

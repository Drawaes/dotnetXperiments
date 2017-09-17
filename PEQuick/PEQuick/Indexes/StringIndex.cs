﻿using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class StringIndex : SimpleIndex
    {
        private string _value;

        public int Index => (int)_rawIndex;
        public string Value => _value;
        public override uint TableOffset => (uint)TableFlag.Strings << 24;

        internal override void Resolve(MetaDataTables tables)
        {
            _value = tables.Strings.GetString(_rawIndex);
        }       
    }
}

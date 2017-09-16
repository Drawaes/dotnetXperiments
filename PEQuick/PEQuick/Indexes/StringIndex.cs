using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class StringIndex : Index
    {
        private string _value;
        
        public int Index => (int)_rawIndex;
        public string Value => _value;

        internal override void Resolve(MetaDataTables tables)
        {
            _value = tables.Strings.GetString(_rawIndex);
        }
    }
}

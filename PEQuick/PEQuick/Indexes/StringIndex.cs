using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class StringIndex : IIndex
    {
        private uint _rawIndex;
        private string _value;
        private bool _resolved;

        public int Index => (int)_rawIndex;

        public void Resolve(MetaDataTables tables)
        {
            throw new NotImplementedException();
        }

        public void SetRawIndex(uint rawIndex)
        {
            _rawIndex = rawIndex;
        }

        public string Value => _value;
    }
}

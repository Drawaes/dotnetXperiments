using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public struct TypeRefRow
    {
        private ScopeIndex _resolutionScope;
        private uint _name;
        private uint _namespace;

        public TypeRefRow(ref MetaDataReader reader)
        {
            _resolutionScope = new ScopeIndex(ref reader);
            _name = reader.ReadStringOffset();
            _namespace = reader.ReadStringOffset();
        }
    }
}

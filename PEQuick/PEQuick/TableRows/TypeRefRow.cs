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
        private StringIndex _name;
        private StringIndex _namespace;

        public TypeRefRow(ref MetaDataReader reader)
        {
            _resolutionScope = new ScopeIndex(ref reader);
            _name = reader.ReadIndex<StringIndex>();
            _namespace = reader.ReadIndex<StringIndex>();
        }
    }
}

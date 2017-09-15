using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class TypeRefRow : Row
    {
        private ResolutionScopeIndex _resolutionScope;
        private StringIndex _nameIndex;
        private StringIndex _namespaceIndex;

        public override void Read(ref MetaDataReader reader)
        {
            _resolutionScope = new ResolutionScopeIndex(ref reader);
            _nameIndex = reader.ReadIndex<StringIndex>();
            _namespaceIndex = reader.ReadIndex<StringIndex>();
        }
    }
}

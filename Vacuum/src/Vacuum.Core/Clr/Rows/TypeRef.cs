using System;
using System.Collections.Generic;
using System.Text;
using Vacuum.Core.Clr.Flags;
using Vacuum.Core.Clr.Indexes;

namespace Vacuum.Core.Clr.Rows
{
    public class TypeRef : Row
    {
        private ResolutionScopeIndex _resolutionScope;
        private StringIndex _nameIndex;
        private StringIndex _namespaceIndex;

        public TypeRef() : base()
        {
        }

        public override TableFlag Flag => TableFlag.TypeRef;

        internal override void LoadFromReader(ref ClrMetaReader reader, int index)
        {
            _index = index;
            _resolutionScope = reader.ReadIndex<ResolutionScopeIndex>();
            _nameIndex = reader.ReadIndex<StringIndex>();
            _namespaceIndex = reader.ReadIndex<StringIndex>();
        }

        internal override void Resolve(ClrData clrData)
        {
            _resolutionScope.Resolve(clrData);
            _nameIndex.Resolve(clrData);
            _namespaceIndex.Resolve(clrData);
        }
    }
}

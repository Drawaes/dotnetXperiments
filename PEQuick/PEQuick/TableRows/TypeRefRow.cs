using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Importer;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class TypeRefRow : Row
    {
        private ResolutionScopeIndex _resolutionScope;
        private StringIndex _nameIndex;
        private StringIndex _namespaceIndex;

        public override TableFlag Table => TableFlag.TypeRef;
        public override uint AssemblyTag => _resolutionScope.Row.AssemblyTag;

        public override void Resolve(MetaDataTables tables)
        {
            _resolutionScope.Resolve(tables);
            _nameIndex.Resolve(tables);
            _namespaceIndex.Resolve(tables);
        }

        public override void Read(ref MetaDataReader reader)
        {
            _resolutionScope = reader.ReadIndex<ResolutionScopeIndex>();
            _nameIndex = reader.ReadIndex<StringIndex>();
            _namespaceIndex = reader.ReadIndex<StringIndex>();
        }

        public override void GetDependencies(DependencyGather tagQueue)
        {
            tagQueue.SeedTag(_resolutionScope.Row);
        }
    }
}

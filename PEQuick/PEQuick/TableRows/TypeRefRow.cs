using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;
using PEQuick.Importer;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class TypeRefRow : Row
    {
        

        public override TableFlag Table => TableFlag.TypeRef;
        public override uint AssemblyTag => _resolutionScope.Row.AssemblyTag;

        public override void Resolve(MetaDataTables tables)
        {
            _resolutionScope.Resolve(tables);
            _nameIndex.Resolve(tables);
            _namespaceIndex.Resolve(tables);
        }
        
        public override void GetDependencies(DependencyGather tagQueue)
        {
            tagQueue.SeedTag(_resolutionScope.Row);
        }
                
        public override void WriteRow(ref MetaDataWriter writer, Dictionary<uint, uint> tokenRemapping)
        {
            writer.WriteIndex(_resolutionScope);
            writer.WriteIndex(_nameIndex);
            writer.WriteIndex(_namespaceIndex);
        }
    }
}

using System.Collections.Generic;
using PEQuick.Flags;
using PEQuick.Importer;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class AssemblyRefRow : Row
    {
        
        public override void Resolve(MetaDataTables tables)
        {
            _publicKeyOrToken.Resolve(tables);
            Name.Resolve(tables);
            _cultureIndex.Resolve(tables);
            _hashValue.Resolve(tables);
        }
        
        public override uint AssemblyTag => Tag;

        public override void GetDependencies(DependencyGather tagQueue)
        {
            //Nothing to add
        }

        public override void WriteRow(ref MetaDataWriter writer, Dictionary<uint, uint> tokenRemapping)
        {
            writer.Write(_majorVersion);
            writer.Write(_minorVersion);
            writer.Write(_buildNumber);
            writer.Write(_revisionNumber);
            writer.Write(_flags);
            writer.WriteIndex(_publicKeyOrToken);
            writer.WriteIndex(Name);
            writer.WriteIndex(_cultureIndex);
            writer.WriteIndex(_hashValue);
        }
    }
}

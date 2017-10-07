using System.Collections.Generic;
using PEQuick.Flags;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class AssemblyRow : Row
    {
        

        public override uint AssemblyTag => Tag;

        public override void Resolve(MetaDataTables tables)
        {
            PublicKey.Resolve(tables);
            Name.Resolve(tables);
            CultureIndex.Resolve(tables);
        }
        
        public override void WriteRow(ref MetaDataWriter writer, Dictionary<uint, uint> tokenRemapping)
        {
            writer.Write(HashAlgId);
            writer.Write(MajorVersion);
            writer.Write(MinorVersion);
            writer.Write(BuildNumber);
            writer.Write(RevisionNumber);
            writer.Write(Flags);
            writer.WriteIndex(PublicKey);
            writer.WriteIndex(Name);
            writer.WriteIndex(CultureIndex);
        }
    }
}

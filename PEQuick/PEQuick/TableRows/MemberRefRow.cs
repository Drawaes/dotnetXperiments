using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;
using PEQuick.Importer;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class MemberRefRow : Row
    {
        public override uint AssemblyTag => _class.Row.AssemblyTag;
        public string Name => _nameIndex.Value;
        public Span<byte> Signature => _signature.Value.AsSpan();

        public override void Resolve(MetaDataTables tables)
        {
            _class.Resolve(tables);
            _nameIndex.Resolve(tables);
            _signature.Resolve(tables);
        }
        
        public override void GetDependencies(DependencyGather tagQueue)
        {
            tagQueue.SeedTag(_class.Row);
            //TODO parse signature;
        }

        public override void WriteRow(ref MetaDataWriter writer, Dictionary<uint, uint> tokenRemapping)
        {
            writer.WriteIndex(_class);
            writer.WriteIndex(_nameIndex);
            writer.WriteIndex(_signature);
        }
    }
}

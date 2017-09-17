using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Importer;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class MemberRefRow : Row
    {
        private MemberRefParentIndex _class;
        private StringIndex _nameIndex;
        private BlobIndex _signature;

        public override TableFlag Table => TableFlag.MemberRef;
        public override uint AssemblyTag => _class.Row.AssemblyTag;
        public string Name => _nameIndex.Value;
        public Span<byte> Signature => _signature.Value.AsSpan();

        public override void Resolve(MetaDataTables tables)
        {
            _class.Resolve(tables);
            _nameIndex.Resolve(tables);
            _signature.Resolve(tables);
        }

        public override void Read(ref MetaDataReader reader)
        {
            _class = reader.ReadIndex<MemberRefParentIndex>();
            _nameIndex = reader.ReadIndex<StringIndex>();
            _signature = reader.ReadIndex<BlobIndex>();
        }

        public override void GetDependencies(DependencyGather tagQueue)
        {
            tagQueue.SeedTag(_class.Row);
            //TODO parse signature;
        }
    }
}

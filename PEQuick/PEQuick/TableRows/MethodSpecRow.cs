using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Importer;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class MethodSpecRow : Row
    {
        private MethodDefOrRefIndex _method;
        private BlobIndex _instantiation;

        public override TableFlag Table => TableFlag.MethodSpec;
        public override uint AssemblyTag => _method.Row.AssemblyTag;

        public override void Resolve(MetaDataTables tables)
        {
            _method.Resolve(tables);
            _instantiation.Resolve(tables);
        }

        public override void Read(ref MetaDataReader reader)
        {
            _method = reader.ReadIndex<MethodDefOrRefIndex>();
            _instantiation = reader.ReadIndex<BlobIndex>();
        }

        public override void GetDependencies(DependencyGather tagQueue)
        {
            tagQueue.SeedTag(_method.Row);
            //TODO calculate anything extra from the instantiation
        }
    }
}

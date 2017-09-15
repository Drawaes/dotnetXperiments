using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class MethodSpecRow : Row
    {
        private MethodDefOrRefIndex _method;
        private BlobIndex _instantiation;

        public override void Read(ref MetaDataReader reader)
        {
            _method = reader.ReadIndex<MethodDefOrRefIndex>();
            _instantiation = reader.ReadIndex<BlobIndex>();
        }
    }
}

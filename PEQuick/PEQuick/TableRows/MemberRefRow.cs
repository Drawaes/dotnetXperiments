using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public struct MemberRefRow
    {
        private MemberRefParentIndex _class;
        private StringIndex _name;
        private BlobIndex _signature;

        public MemberRefRow(ref MetaDataReader reader)
        {
            _class = reader.ReadIndex<MemberRefParentIndex>();
            _name = reader.ReadIndex<StringIndex>();
            _signature = reader.ReadIndex<BlobIndex>();
        }
    }
}

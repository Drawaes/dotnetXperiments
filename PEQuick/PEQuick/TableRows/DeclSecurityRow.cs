using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public struct DeclSecurityRow : IRow
    {
        private ushort _action;
        private HasDeclSecurityIndex _parent;
        private BlobIndex _permissionSet;

        public void Read(ref MetaDataReader reader)
        {
            _action = reader.Read<ushort>();
            _parent = reader.ReadIndex<HasDeclSecurityIndex>();
            _permissionSet = reader.ReadIndex<BlobIndex>();
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class DeclSecurityRow : Row
    {
        private ushort _action;
        private HasDeclSecurityIndex _parent;
        private BlobIndex _permissionSet;

        public override TableFlag Table => TableFlag.DeclSecurity;
        public override uint AssemblyTag => _parent.Row.AssemblyTag;

        public override void Resolve(MetaDataTables tables)
        {
            _parent.Resolve(tables);
            _permissionSet.Resolve(tables);
        }

        public override void Read(ref MetaDataReader reader)
        {
            _action = reader.Read<ushort>();
            _parent = reader.ReadIndex<HasDeclSecurityIndex>();
            _permissionSet = reader.ReadIndex<BlobIndex>();
        }

        public override void WriteRow(ref MetaDataWriter writer, Dictionary<uint, uint> tokenRemapping)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class ClassLayoutRow : Row
    {
        private ushort _packingSize;
        private uint _classSize;
        private TypeDefIndex _parent;

        public override void Read(ref MetaDataReader reader)
        {
            _packingSize = reader.Read<ushort>();
            _classSize = reader.Read<uint>();
            _parent = reader.ReadIndex<TypeDefIndex>();
        }
    }
}

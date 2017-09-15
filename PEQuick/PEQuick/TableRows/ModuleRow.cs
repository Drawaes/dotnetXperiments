using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class ModuleTableRow : Row
    {
        public ushort Generation;
        private StringIndex _nameIndex;
        public uint Mvid;
        public uint EncId;
        public uint EncBaseId;

        public override void Read(ref MetaDataReader reader)
        {
            Generation = reader.Read<ushort>();
            _nameIndex = reader.ReadIndex<StringIndex>();
            Mvid = reader.ReadGuidOffset();
            EncId = reader.ReadGuidOffset();
            EncBaseId = reader.ReadGuidOffset();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ModuleTableRow:IRow
    {
        public ushort Generation;
        public StringIndex Name;
        public uint Mvid;
        public uint EncId;
        public uint EncBaseId;
                
        public void Read(ref MetaDataReader reader)
        {
            Generation = reader.Read<ushort>();
            Name = reader.ReadIndex<StringIndex>();
            Mvid = reader.ReadGuidOffset();
            EncId = reader.ReadGuidOffset();
            EncBaseId = reader.ReadGuidOffset();
        }
    }
}
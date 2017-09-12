using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ModuleTableRow
    {
        public ushort Generation;
        public uint Name;
        public uint Mvid;
        public uint EncId;
        public uint EncBaseId;

        public ModuleTableRow(ref MetaDataReader reader)
        {
            Generation = reader.Read<ushort>();
            Name = reader.ReadStringOffset();
            Mvid = reader.ReadGuidOffset();
            EncId = reader.ReadGuidOffset();
            EncBaseId = reader.ReadGuidOffset();
        }
    }
}
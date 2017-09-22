using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Vacuum.Core.PE;

namespace Vacuum.Core.Clr
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ClrHeader
    {
        public uint CB;
        public ushort MajorRuntimeVersion;
        public ushort MinorRuntimeVersion;
        public ImageDataDirectory MetaData;
        public uint Flags;
        public uint EntryPointToken;
        public ImageDataDirectory Resources;
        public ImageDataDirectory StrongNameSignatures;
        public ImageDataDirectory CodeManagerTable;
        public ImageDataDirectory VTableFixups;
        public ImageDataDirectory ExportAddressTableJumps;
        public ImageDataDirectory ManagedNativeHeader;
    }
}

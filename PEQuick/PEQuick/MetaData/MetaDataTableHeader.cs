using System;
using System.Collections.Generic;
using System.Text;

namespace PEQuick.MetaData
{
    public struct MetaDataTableHeader
    {
        public uint Reserved1;
        public byte MajorVersion;
        public byte MinorVersion;
        public HeapOffsetSizeFlags HeapOffsetSizeFlags;
        public byte Reserved2;
        public MetadataTableFlags TablesFlags;
        public MetadataTableFlags SortedTablesFlags;
    }
}

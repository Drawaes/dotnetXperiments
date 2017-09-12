using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace PEQuick
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DataDirectoriesHeader
    {
        public ulong ExportTable;
        public ulong ImportTable;
        public ulong ResourceTable;
        public ulong ExceptionTable;
        public ulong CertificateTable;
        public ulong BaseRelocationTable;
    }
}

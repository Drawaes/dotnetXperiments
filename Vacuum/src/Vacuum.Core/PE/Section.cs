using System;
using System.Collections.Generic;
using System.Text;

namespace Vacuum.Core.PE
{
    public class Section
    {
        private SectionHeader _header;
        private byte[] _file;

        public Section(byte[] file, SectionHeader header)
        {
            _header = header;
            _file = file;
        }

        public ReadOnlySpan<byte> GetSpan() => _file.AsReadOnlySpan().Slice(_header.PointerToRawData, _header.VirtualSize);

        public bool InRange(int rva) => rva >= _header.VirtualAddress && rva < _header.VirtualAddress + _header.VirtualSize;

        public ReadOnlySpan<byte> GetSpan(ImageDataDirectory directory)
        {
            var actualAddress = directory.VirtualAddress - _header.VirtualAddress + _header.PointerToRawData;
            return _file.AsReadOnlySpan().Slice(actualAddress, directory.Size);
        }
    }
}

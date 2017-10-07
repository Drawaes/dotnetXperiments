using System;
using System.Collections.Generic;
using System.Text;

namespace PEQuick.Output
{
    public class SectionDump
    {
        private int _virtualAddress;
        private int _sectionAlign;
        private byte[] _buffer;
        private int _currentIndex;

        public SectionDump(int virtualAddress, int sectionAlign)
        {
            _virtualAddress = virtualAddress;
            _sectionAlign = sectionAlign;
            _buffer = new byte[_sectionAlign * 1024];
        }

        public uint VirtualAddress => (uint)_virtualAddress;
        public int CurrentIndex => _currentIndex + _virtualAddress;
        public uint RawSize => Utils.Align((uint)_currentIndex, (uint)512);
        public uint VirtualSize => Utils.Align((uint)(_currentIndex + 1),512);

        public int WriteData(Span<byte> dataToWrite)
        {
            var returnIndex = _virtualAddress + _currentIndex;
            dataToWrite.CopyTo(_buffer.AsSpan().Slice(_currentIndex));
            _currentIndex += dataToWrite.Length;
            return returnIndex;
        }

        internal Span<byte> ToSpan() => new Span<byte>(_buffer, 0, (int)VirtualSize);
    }
}

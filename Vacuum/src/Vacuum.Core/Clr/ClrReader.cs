using System;
using System.Collections.Generic;
using System.Text;

namespace Vacuum.Core.Clr
{
    public struct ClrReader
    {
        private ReadOnlySpan<byte> _innerSpan;
        private ReadOnlySpan<byte> _currentSpan;
        private const byte OneByteFilter = 0b1000_0000;
        private const byte TwoByteFilter = 0b0100_0000;
        private const byte FourByteFilter = OneByteFilter | TwoByteFilter;

        public ClrReader(ReadOnlySpan<byte> innerSpan)
        {
            _innerSpan = innerSpan;
            _currentSpan = innerSpan;
        }

        public ReadOnlySpan<byte> CurrentSpan => _currentSpan;
        
        public int Remaining => _currentSpan.Length;
        public int CurrentIndex => _innerSpan.Length - _currentSpan.Length;

        public void Advance(int numberOfBytes) => _currentSpan = _currentSpan.Slice(numberOfBytes);

        public void CheckMagicNumber<T>(T value)
            where T : struct => _currentSpan = _currentSpan.CheckForMagicValue(value);

        public T Read<T>()
            where T : struct
        {
            _currentSpan = _currentSpan.Read<T>(out T value);
            return value;
        }

        public unsafe string ReadFixedLengthAscii(int length)
        {
            string returnValue;
            fixed (byte* s = &_currentSpan.DangerousGetPinnableReference())
            {
                returnValue = Encoding.ASCII.GetString(s, length);
            }
            _currentSpan = _currentSpan.Slice(length);
            return returnValue;
        }
        
        public unsafe string ReadFixedLengthUtf16(int length)
        {
            var adjustment = length % 2;
            string returnValue;
            fixed (byte* ptr = &_currentSpan.DangerousGetPinnableReference())
            {
                returnValue = new string((char*)ptr, 0, (length - adjustment)/2);
            }
            _currentSpan = _currentSpan.Slice(length);
            return returnValue;
        }

        public unsafe string ReadAlignedString()
        {
            var nullIndex = _currentSpan.IndexOf(0);
            string returnValue;
            fixed (byte* s = &_currentSpan.DangerousGetPinnableReference())
            {
                returnValue = Encoding.ASCII.GetString(s, nullIndex + 1);
            }
            nullIndex = (int)Utils.Utils.Align((uint)nullIndex + 1, 4);
            _currentSpan = _currentSpan.Slice(nullIndex);
            return returnValue;
        }

        public unsafe string ReadNullTerminatedAscii()
        {
            var nullIndex = _currentSpan.IndexOf(0);
            string returnValue;
            fixed (byte* s = &_currentSpan.DangerousGetPinnableReference())
            {
                returnValue = Encoding.ASCII.GetString(s, nullIndex);
            }
            _currentSpan = _currentSpan.Slice(nullIndex + 1);
            return returnValue;
        }

        public uint ReadEncodedInt()
        {
            uint returnValue;
            int sliceSize;
            if ((_currentSpan[0] & OneByteFilter) == 0)
            {
                returnValue = _currentSpan[0];
                sliceSize = 1;
            }
            else if ((_currentSpan[0] & TwoByteFilter) == 0)
            {
                returnValue = (uint)(((_currentSpan[0] & ~OneByteFilter) << 8) | _currentSpan[1]);
                sliceSize = 2;
            }
            else
            {
                returnValue = (uint)(((_currentSpan[0] & ~FourByteFilter) << 24)
                    | (_currentSpan[1] << 16)
                    | (_currentSpan[2] << 8)
                    | _currentSpan[3]);
                sliceSize = 4;
            }
            _currentSpan = _currentSpan.Slice(sliceSize);
            return returnValue;
        }
    }
}

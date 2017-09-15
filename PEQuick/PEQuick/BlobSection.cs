using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;

namespace PEQuick
{
    public class BlobSection
    {
        private byte[] _section;

        public BlobSection(Span<byte> input)
        {
            _section = input.ToArray();
        }

        public int ReadInt(ref Span<byte> input)
        {
            const byte maxSizeFilter = 0b1000_0000;
            const byte valueFilter = 0b0111_1111;

            bool loop = false;
            var returnValue = 0;
            var counter = 0;
            do
            {
                var currentValue = input[counter];
                returnValue >>= 8;
                returnValue += (currentValue & valueFilter);
                loop = ((currentValue & maxSizeFilter) != 0);
                counter++;
            } while (loop);
            input = input.Slice(counter);
            return returnValue;
        }

        public byte[] GetBlob(BlobIndex blobIndex)
        {
            var span = _section.AsSpan().Slice(blobIndex.Index);
            var size = ReadInt(ref span);
            return span.Slice(0, size).ToArray();
        }
    }
}

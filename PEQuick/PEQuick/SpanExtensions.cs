using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace PEQuick
{
    public static class SpanExtensions
    {
        public unsafe static Span<byte> Read<T>(this Span<byte> span, out T value) where T : struct
        {
            var size = Unsafe.SizeOf<T>();
            var val = default(T);
            var tempSpan = new Span<byte>(Unsafe.AsPointer(ref val), size);
            span.Slice(0, size).CopyTo(tempSpan);
            value = val;
            return span.Slice(size);
        }

        public static Span<byte> ReadLengthPrefixedString(this Span<byte> input, out string value)
        {
            input = input.Read(out uint size);
            var i = input.IndexOf(0);
            var stringSpan = input.Slice(0,Math.Min(i, (int)size));
            value = stringSpan.ReadNullString();
            return input.Slice((int)size);
        }

        public unsafe static string ReadNullString(this Span<byte> input)
        {
            fixed (void* ptr = &input.DangerousGetPinnableReference())
            {
                return Marshal.PtrToStringUTF8((IntPtr)ptr, input.Length);
            }
        }

        public unsafe static Span<byte> CheckForMagicValue<T>(this Span<byte> span, T magicNumber) where T : struct
        {
            var val = magicNumber;
            var size = Unsafe.SizeOf<T>();
            var tempSpan = new Span<byte>(Unsafe.AsPointer(ref val), size);
            if (!span.Slice(0, size).SequenceEqual(tempSpan))
            {
                throw new InvalidOperationException();
            }
            return span.Slice(size);
        }

        public static Span<byte> ReadStream(this Span<byte> input, out StreamHeader streamHeader)
        {
            var header = new StreamHeader();
            input = input.Read(out header.Offset);
            input = input.Read(out header.Size);
            input = input.ReadAlignedString(out header.Name);
            streamHeader = header;
            return input;
        }

        public static Span<byte> ReadMetadataHeader(this Span<byte> data, out CliDataHeader metaData)
        {
            metaData = new CliDataHeader();
            data = data.Read(out metaData.MajorVersion);
            data = data.Read(out metaData.MinorVersion);
            data = data.Read(out metaData.Reserved);
            data = data.ReadLengthPrefixedString(out metaData.Version);
            data = data.Read(out metaData.Flags);
            data = data.Read(out metaData.Streams);
            return data;
        }

        public unsafe static Span<byte> ReadAlignedString(this Span<byte> input, out string value)
        {
            var nullTerminator = input.IndexOf(0);
            fixed(void* ptr = &input.DangerousGetPinnableReference())
            {
                value = Marshal.PtrToStringUTF8((IntPtr)ptr, nullTerminator);
            }
            // align to 4 byte boundary
            nullTerminator = (nullTerminator + 3) & ~0x03;
            return input.Slice(nullTerminator);
        }

        public static int BitCount(ulong value)
        {
            // see https://github.com/dotnet/corefx/blob/5965fd3756bc9dd9c89a27621eb10c6931126de2/src/System.Reflection.Metadata/src/System/Reflection/Internal/Utilities/BitArithmetic.cs

            const ulong Mask01010101 = 0x5555555555555555UL;
            const ulong Mask00110011 = 0x3333333333333333UL;
            const ulong Mask00001111 = 0x0F0F0F0F0F0F0F0FUL;
            const ulong Mask00000001 = 0x0101010101010101UL;

            var v = (ulong)value;

            v = v - ((v >> 1) & Mask01010101);
            v = (v & Mask00110011) + ((v >> 2) & Mask00110011);
            return (int)(unchecked(((v + (v >> 4)) & Mask00001111) * Mask00000001) >> 56);
        }
    }
}

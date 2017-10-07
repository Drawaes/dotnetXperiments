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

        public unsafe static Span<byte> Write<T>(this Span<byte> input, T[] values)
            where T : struct
        {
            var size = Unsafe.SizeOf<T>() * values.Length;
            if (input.Length < size)
            {
                throw new InvalidOperationException();
            }
            var span = new Span<T>(values).AsBytes();
            span.CopyTo(input);
            input = input.Slice(size);
            return input;
        }

        
        const byte oneByteLimit = oneByteFilter;
        const ushort twoByteLimit = twoByteFilter << 8;
        

        public static Span<byte> WriteEncodedInt(this Span<byte> input, uint value)
        {
            if (value < oneByteLimit)
            {
                input[0] = (byte)value;
                return input.Slice(1);
            }

            if (value < twoByteLimit)
            {
                input[0] = (byte)((value >> 8) | oneByteFilter);
                input[1] = (byte)(value & 0xff);
                return input.Slice(2);
            }

            input[0] = (byte)((value >> 24) | fourByteFilter);
            input[1] = (byte)((value >> 16) | 0xff);
            input[2] = (byte)((value >> 8) | 0xff);
            input[3] = (byte)(value | 0xff);
            return input.Slice(4);
        }

        

        public static Span<byte> ReadLengthPrefixedString(this Span<byte> input, out string value)
        {
            input = input.Read(out uint size);
            value = input.ReadNullTerminatedString();
            return input.Slice((int)size);
        }

        public unsafe static string ReadNullTerminatedString(this Span<byte> input)
        {
            var length = Math.Min(input.Length, input.IndexOf(0));
            fixed (void* ptr = &input.DangerousGetPinnableReference())
            {
                return Marshal.PtrToStringUTF8((IntPtr)ptr, length);
            }
        }

        public static Span<byte> WriteLengthPrefixedString(this Span<byte> output, string value)
        {
            var length = value.Length + 1;
            output = output.Write((uint)length);
            output = output.WriteNullTerminatedString(value);
            return output;
        }

        public unsafe static Span<byte> WriteNullTerminatedString(this Span<byte> output, string value)
        {
            var length = value.Length + 1;
            if (length > output.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Error not enough space to write");
            }
            fixed (char* c = value)
            fixed (byte* o = &output.DangerousGetPinnableReference())
            {
                var amountWritten = Encoding.UTF8.GetBytes(c, value.Length, o, output.Length);
                output = output.Slice(amountWritten);
                output[0] = 0;
                return output.Slice(1);
            }
        }

        public unsafe static Span<byte> Write<T>(this Span<byte> input, T value)
            where T : struct
        {
            var length = Unsafe.SizeOf<T>();
            if (input.Length < length)
            {
                throw new NotImplementedException();
            }
            fixed (void* ptr = &input.DangerousGetPinnableReference())
            {
                Unsafe.Write(ptr, value);
            }
            return input.Slice(length);
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
            fixed (void* ptr = &input.DangerousGetPinnableReference())
            {
                value = Marshal.PtrToStringUTF8((IntPtr)ptr, nullTerminator);
            }
            // align to 4 byte boundary
            nullTerminator = (int)Utils.Align((uint)nullTerminator + 1, 4);
            return input.Slice(nullTerminator);
        }

        public unsafe static Span<byte> WriteAlignedString(this Span<byte> output, string value)
        {
            var sliceLength = (uint)value.Length + 1;
            sliceLength = Utils.Align(sliceLength, 4);
            output = output.WriteNullTerminatedString(value);
            for(var i = 0; i < sliceLength - (value.Length + 1);i++)
            {
                output[i] = 0;
            }
            return output.Slice((int)sliceLength - (value.Length + 1));
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

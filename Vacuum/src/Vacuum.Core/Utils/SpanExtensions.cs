using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Vacuum.Core
{
    public static class SpanExtensions
    {
        public unsafe static ReadOnlySpan<byte> CheckForMagicValue<T>(this ReadOnlySpan<byte> span, T magicNumber)
            where T : struct
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

        public unsafe static ReadOnlySpan<byte> Read<T>(this ReadOnlySpan<byte> span, out T value)
            where T : struct
        {
            var size = Unsafe.SizeOf<T>();
            value = Unsafe.As<byte, T>(ref span.DangerousGetPinnableReference());
            return span.Slice(size);
        }
    }
}

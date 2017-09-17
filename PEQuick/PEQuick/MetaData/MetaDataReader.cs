using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using PEQuick.Flags;
using PEQuick.Indexes;

namespace PEQuick.MetaData
{
    public struct MetaDataReader
    {
        private Span<byte> _input;
        private Dictionary<Type, bool> _useLargeIndexes;
        private int _initialLength;

        public MetaDataReader(Span<byte> input, HeapOffsetSizeFlags heapOffsetSizes, Dictionary<TableFlag, int> sizes)
        {
            _initialLength = input.Length;
            _input = input;

            _useLargeIndexes = TagSizes.GetLargeSizes((f) => sizes.GetSize(f), heapOffsetSizes);
        }

        public int Length => _input.Length;
        public int Index => _initialLength - Length;

        private static bool UsesLargeIndex(Dictionary<TableFlag, int> sizes, TableFlag flags)
        {
            var maxItems = sizes.GetSize(flags);
            return maxItems >= ushort.MaxValue;
        }

        private static bool UsesLargeIndexes(Dictionary<TableFlag, int> sizes, int bits, params TableFlag[] flags)
        {
            var max = 0;
            for (var i = 0; i < flags.Length; i++)
            {
                max = Math.Max(max, sizes.GetSize(flags[i]));
            }
            max <<= bits;
            return max >= ushort.MaxValue;
        }

        public T ReadIndex<T>() where T : Index, new()
        {
            uint value;
            if (_useLargeIndexes[typeof(T)])
            {
                value = Read<uint>();
            }
            else
            {
                value = Read<ushort>();
            }
            var newValue = new T();
            newValue.SetRawIndex(value);
            return newValue;
        }
        
        public T Read<T>() where T : struct
        {
            Debug.Assert(!typeof(Index).IsAssignableFrom(typeof(T)));

            _input = _input.Read(out T value);
            return value;
        }
    }
}

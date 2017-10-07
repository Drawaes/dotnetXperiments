using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.Output;

namespace PEQuick.MetaData
{
    public struct MetaDataWriter
    {
        private Span<byte> _input;
        private Dictionary<Type, bool> _useLargeIndexes;
        private int _initialLength;
        private Dictionary<uint, uint> _remapper;
        private SectionDump _dataSection;

        public MetaDataWriter(SectionDump dataSection, Span<byte> input, HeapOffsetSizeFlags heapSizes, MetaDataTables tables, Dictionary<uint, uint> remapper)
        {
            _dataSection = dataSection;
            _initialLength = input.Length;
            _input = input;
            _remapper = remapper;

            _useLargeIndexes = TagSizes.GetLargeSizes((f) => tables.GetTableSize(f), heapSizes);
        }

        public int Length => _input.Length;

        public void WriteIndex<T>(T index)
            where T : Index
        {
            _input = index.Write(_input, _remapper, _useLargeIndexes[typeof(T)]);
        }

        public void Write<T>(T value)
            where T : struct
        {
            _input = _input.Write(value);
        }

        public int WriteRVABlob(Span<byte> blob) => _dataSection.WriteData(blob);
    }
}

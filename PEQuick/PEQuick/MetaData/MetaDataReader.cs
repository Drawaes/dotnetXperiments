using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;

namespace PEQuick.MetaData
{
    public struct MetaDataReader
    {
        private Span<byte> _input;
        private bool _largeStringOffsets;
        private bool _largeGuidOffsets;
        private bool _largeBlobOffsets;
        private Dictionary<MetadataTableFlags, int> _sizes;
        private bool _largeTypeDefOrRef;

        public MetaDataReader(Span<byte> input, HeapOffsetSizeFlags heapOffsetSizes, Dictionary<MetadataTableFlags, int> sizes)
        {
            _input = input;
            _largeStringOffsets = (heapOffsetSizes & HeapOffsetSizeFlags.String) != 0;
            _largeGuidOffsets = (heapOffsetSizes & HeapOffsetSizeFlags.GUID) != 0;
            _largeBlobOffsets = (heapOffsetSizes & HeapOffsetSizeFlags.Blob) != 0;
            _sizes = sizes;

            _largeTypeDefOrRef = UsesLargeTypeDefOrRef(_sizes);
        }

        public int Length => _input.Length;

        private static bool UsesLargeTypeDefOrRef(Dictionary<MetadataTableFlags, int> sizes)
        {
            var defOrRef = Math.Max(sizes.GetSize(MetadataTableFlags.TypeRef), sizes.GetSize(MetadataTableFlags.TypeDef));
            defOrRef = Math.Max(defOrRef, sizes.GetSize(MetadataTableFlags.TypeSpec)) << 2;
            return defOrRef >= ushort.MaxValue;
        }

        public TypeDefOrRefIndex ReadTypeDefOrRefIndex()
        {
            if (_largeTypeDefOrRef)
            {
                return new TypeDefOrRefIndex(Read<uint>());
            }
            return new TypeDefOrRefIndex(Read<ushort>());
        }

        public uint ReadStringOffset()
        {
            if (_largeStringOffsets)
            {
                _input = _input.Read(out uint bigValue);
                return bigValue;
            }
            _input = _input.Read(out ushort value);
            return value;
        }

        public uint ReadBlobOffset()
        {
            if (_largeBlobOffsets)
            {
                _input = _input.Read(out uint bigValue);
                return bigValue;
            }
            _input = _input.Read(out ushort value);
            return value;
        }

        public uint ReadGuidOffset()
        {
            if (_largeGuidOffsets)
            {
                _input = _input.Read(out uint bigValue);
                return bigValue;
            }
            _input = _input.Read(out ushort value);
            return value;
        }

        public T Read<T>() where T : struct
        {
            _input = _input.Read(out T value);
            return value;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;

namespace PEQuick.MetaData
{
    public struct MetaDataReader
    {
        private Span<byte> _input;
        private bool _largeGuidOffsets;
        private Dictionary<MetadataTableFlags, int> _sizes;
        private Dictionary<Type, bool> _useLargeIndexes;
        private int _initialLength;

        public MetaDataReader(Span<byte> input, HeapOffsetSizeFlags heapOffsetSizes, Dictionary<MetadataTableFlags, int> sizes)
        {
            _initialLength = input.Length;
            _input = input;
            _largeGuidOffsets = (heapOffsetSizes & HeapOffsetSizeFlags.GUID) != 0;
            _sizes = sizes;

            _useLargeIndexes = new Dictionary<Type, bool>
            {
                { typeof(HasSemanticsIndex), UsesLargeIndexes(_sizes, 1, MetadataTableFlags.Event, MetadataTableFlags.Property) },
                { typeof(HasDeclSecurityIndex), UsesLargeIndexes(_sizes, 2, MetadataTableFlags.TypeDef, MetadataTableFlags.Method, MetadataTableFlags.Assembly) },
                { typeof(StringIndex), (heapOffsetSizes & HeapOffsetSizeFlags.String) != 0 },
                { typeof(BlobIndex), (heapOffsetSizes & HeapOffsetSizeFlags.Blob) != 0 },
                { typeof(HasFieldMarshalIndex), UsesLargeIndexes(_sizes, 1, MetadataTableFlags.Field, MetadataTableFlags.Param) },
                { typeof(CustomAttributeTypeIndex), UsesLargeIndexes(_sizes, 3, MetadataTableFlags.Method, MetadataTableFlags.MemberRef) },
                { typeof(HasCustomAttributeIndex), UsesLargeIndexes(_sizes, 5, MetadataTableFlags.Method, MetadataTableFlags.TypeDef, MetadataTableFlags.TypeRef,
                    MetadataTableFlags.Field, MetadataTableFlags.Param, MetadataTableFlags.InterfaceImpl, MetadataTableFlags.MemberRef, MetadataTableFlags.Module,
                    MetadataTableFlags.DeclSecurity, MetadataTableFlags.Property, MetadataTableFlags.Event, MetadataTableFlags.StandAloneSig, MetadataTableFlags.ModuleRef,
                    MetadataTableFlags.TypeSpec, MetadataTableFlags.Assembly, MetadataTableFlags.AssemblyRef, MetadataTableFlags.File, MetadataTableFlags.ExportedType,
                    MetadataTableFlags.ManifestResource) },
                { typeof(HasConstantIndex), UsesLargeIndexes(_sizes, 2, MetadataTableFlags.Field, MetadataTableFlags.Param, MetadataTableFlags.Property) },
                { typeof(MemberRefParentIndex), UsesLargeIndexes(_sizes, 3, MetadataTableFlags.TypeDef, MetadataTableFlags.TypeRef, MetadataTableFlags.ModuleRef,
                    MetadataTableFlags.Module, MetadataTableFlags.TypeSpec) },
                { typeof(FieldIndex), UsesLargeIndex(_sizes, MetadataTableFlags.Field) },
                { typeof(TypeDefIndex), UsesLargeIndex(_sizes, MetadataTableFlags.TypeDef) },
                { typeof(TypeDefOrRefIndex), UsesLargeIndexes(_sizes, 2 , MetadataTableFlags.TypeRef, MetadataTableFlags.TypeDef, MetadataTableFlags.TypeSpec) },
                { typeof(MethodIndex), UsesLargeIndex(_sizes, MetadataTableFlags.Method) },
                { typeof(ParamIndex), UsesLargeIndex(_sizes, MetadataTableFlags.Param) },
                { typeof(ScopeIndex),  UsesLargeIndexes(_sizes, 4, MetadataTableFlags.AssemblyRef, MetadataTableFlags.Module, MetadataTableFlags.ModuleRef, MetadataTableFlags.TypeRef)},
            };

        }

        public int Length => _input.Length;
        public int Index => _initialLength - Length;

        private static bool UsesLargeIndex(Dictionary<MetadataTableFlags, int> sizes, MetadataTableFlags flags)
        {
            var maxItems = sizes.GetSize(flags);
            return maxItems >= ushort.MaxValue;
        }

        private static bool UsesLargeIndexes(Dictionary<MetadataTableFlags, int> sizes, int bits, params MetadataTableFlags[] flags)
        {
            var max = 0;
            for (var i = 0; i < flags.Length; i++)
            {
                max = Math.Max(max, sizes.GetSize(flags[i]));
            }
            max <<= bits;
            return max >= ushort.MaxValue;
        }

        public T ReadIndex<T>() where T : struct, IIndex
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

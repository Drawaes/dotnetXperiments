using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using PEQuick.Indexes;

namespace PEQuick.MetaData
{
    public struct MetaDataReader
    {
        private Span<byte> _input;
        private bool _largeGuidOffsets;
        private Dictionary<TableFlag, int> _sizes;
        private Dictionary<Type, bool> _useLargeIndexes;
        private int _initialLength;

        public MetaDataReader(Span<byte> input, HeapOffsetSizeFlags heapOffsetSizes, Dictionary<TableFlag, int> sizes)
        {
            _initialLength = input.Length;
            _input = input;
            _largeGuidOffsets = (heapOffsetSizes & HeapOffsetSizeFlags.GUID) != 0;
            _sizes = sizes;

            _useLargeIndexes = new Dictionary<Type, bool>
            {
                { typeof(TypeOrMethodDefIndex), UsesLargeIndexes(_sizes, 1, TableFlag.Method, TableFlag.TypeDef) },
                { typeof(MethodDefOrRefIndex), UsesLargeIndexes(_sizes, 1, TableFlag.Method, TableFlag.MemberRef) },
                {typeof(ImplementationIndex), UsesLargeIndexes(_sizes, 2, TableFlag.File, TableFlag.AssemblyRef, TableFlag.ExportedType) },
                { typeof(MemberForwardedIndex), UsesLargeIndexes(_sizes, 1, TableFlag.Field, TableFlag.Method) },
                { typeof(EventIndex), UsesLargeIndex(_sizes, TableFlag.Event) },
                {typeof(PropertyIndex), UsesLargeIndex(_sizes, TableFlag.Property) },
                { typeof(HasSemanticsIndex), UsesLargeIndexes(_sizes, 1, TableFlag.Event, TableFlag.Property) },
                { typeof(HasDeclSecurityIndex), UsesLargeIndexes(_sizes, 2, TableFlag.TypeDef, TableFlag.Method, TableFlag.Assembly) },
                { typeof(StringIndex), (heapOffsetSizes & HeapOffsetSizeFlags.String) != 0 },
                { typeof(BlobIndex), (heapOffsetSizes & HeapOffsetSizeFlags.Blob) != 0 },
                { typeof(HasFieldMarshalIndex), UsesLargeIndexes(_sizes, 1, TableFlag.Field, TableFlag.Param) },
                { typeof(CustomAttributeTypeIndex), UsesLargeIndexes(_sizes, 3, TableFlag.Method, TableFlag.MemberRef) },
                { typeof(HasCustomAttributeIndex), UsesLargeIndexes(_sizes, 5, TableFlag.Method, TableFlag.TypeDef, TableFlag.TypeRef,
                    TableFlag.Field, TableFlag.Param, TableFlag.InterfaceImpl, TableFlag.MemberRef, TableFlag.Module,
                    TableFlag.DeclSecurity, TableFlag.Property, TableFlag.Event, TableFlag.StandAloneSig, TableFlag.ModuleRef,
                    TableFlag.TypeSpec, TableFlag.Assembly, TableFlag.AssemblyRef, TableFlag.File, TableFlag.ExportedType,
                    TableFlag.ManifestResource) },
                { typeof(HasConstantIndex), UsesLargeIndexes(_sizes, 2, TableFlag.Field, TableFlag.Param, TableFlag.Property) },
                { typeof(MemberRefParentIndex), UsesLargeIndexes(_sizes, 3, TableFlag.TypeDef, TableFlag.TypeRef, TableFlag.ModuleRef,
                    TableFlag.Module, TableFlag.TypeSpec) },
                { typeof(FieldIndex), UsesLargeIndex(_sizes, TableFlag.Field) },
                { typeof(TypeDefIndex), UsesLargeIndex(_sizes, TableFlag.TypeDef) },
                { typeof(TypeDefOrRefIndex), UsesLargeIndexes(_sizes, 2 , TableFlag.TypeRef, TableFlag.TypeDef, TableFlag.TypeSpec) },
                { typeof(MethodIndex), UsesLargeIndex(_sizes, TableFlag.Method) },
                { typeof(ParamIndex), UsesLargeIndex(_sizes, TableFlag.Param) },
                { typeof(ResolutionScopeIndex),  UsesLargeIndexes(_sizes, 4, TableFlag.AssemblyRef, TableFlag.Module, TableFlag.ModuleRef, TableFlag.TypeRef)},
            };

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
            Debug.Assert(!typeof(Index).IsAssignableFrom(typeof(T)));

            _input = _input.Read(out T value);
            return value;
        }
    }
}

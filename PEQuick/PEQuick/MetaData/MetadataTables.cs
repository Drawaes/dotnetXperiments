using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.MetaData
{
    public class MetaDataTables
    {
        private Dictionary<TableFlag, int> _sizes = new Dictionary<TableFlag, int>();
        private StringsSection _strings;
        private BlobSection _blobs;
        private byte _majorVersion;
        private byte _minorVersion;
        private Dictionary<TableFlag, ITable> _tables = new Dictionary<TableFlag, ITable>();
        private Dictionary<Type, ITable> _tablesByType = new Dictionary<Type, ITable>();
        private PEFile _peFile;
        private Dictionary<(uint AssemblyTag, uint Tag), Row> _assemblyIndexedRows;
        private Dictionary<uint, Row> _indexedRows;

        private void LoadEmptyCollections()
        {
            _tables.Add(TableFlag.Module, new Table<ModuleRow>());
            _tables.Add(TableFlag.Method, new Table<MethodRow>());
            _tables.Add(TableFlag.TypeRef, new Table<TypeRefRow>());
            _tables.Add(TableFlag.TypeDef, new Table<TypeDefRow>());
            _tables.Add(TableFlag.Field, new Table<FieldRow>());
            _tables.Add(TableFlag.Param, new Table<ParamRow>());
            _tables.Add(TableFlag.InterfaceImpl, new Table<InterfaceImplementationRow>());
            _tables.Add(TableFlag.MemberRef, new Table<MemberRefRow>());
            _tables.Add(TableFlag.Constant, new Table<ConstantRow>());
            _tables.Add(TableFlag.CustomAttribute, new Table<CustomAttributeRow>());
            _tables.Add(TableFlag.FieldMarshal, new Table<FieldMarshalRow>());
            _tables.Add(TableFlag.AssemblyRef, new Table<AssemblyRefRow>());
            _tables.Add(TableFlag.DeclSecurity, new Table<DeclSecurityRow>());
            _tables.Add(TableFlag.ClassLayout, new Table<ClassLayoutRow>());
            _tables.Add(TableFlag.FieldLayout, new Table<FieldLayoutRow>());
            _tables.Add(TableFlag.StandAloneSig, new Table<StandAloneSigRow>());
            _tables.Add(TableFlag.EventMap, new Table<EventMapRow>());
            _tables.Add(TableFlag.Event, new Table<EventRow>());
            _tables.Add(TableFlag.PropertyMap, new Table<PropertyMapRow>());
            _tables.Add(TableFlag.Property, new Table<PropertyRow>());
            _tables.Add(TableFlag.MethodSemantics, new Table<MethodSemanticsRow>());
            _tables.Add(TableFlag.MethodImpl, new Table<MethodImplRow>());
            _tables.Add(TableFlag.ModuleRef, new Table<ModuleRefRow>());
            _tables.Add(TableFlag.TypeSpec, new Table<TypeSpecRow>());
            _tables.Add(TableFlag.ImplMap, new Table<ImplMapRow>());
            _tables.Add(TableFlag.FieldRVA, new Table<FieldRVA>());
            _tables.Add(TableFlag.Assembly, new Table<AssemblyRow>());
            _tables.Add(TableFlag.ManifestResource, new Table<ManifestResourceRow>());
            _tables.Add(TableFlag.NestedClass, new Table<NestedClassRow>());
            _tables.Add(TableFlag.GenericParam, new Table<GenericParamRow>());
            _tables.Add(TableFlag.MethodSpec, new Table<MethodSpecRow>());
        }

        internal MethodRow FindMethodDef(Row method)
        {
            if (!(method is MemberRefRow member))
            {
                throw new InvalidOperationException();
            }

            var methods = _tables[TableFlag.Method];
            return methods.Cast<MethodRow>().Single(m => m.Name == member.Name && m.Signature.SequenceEqual(member.Signature));
        }

        public MetaDataTables(Span<byte> inputs, PEFile peFile)
        {
            _strings = peFile.Strings;
            _blobs = peFile.Blobs;
            _peFile = peFile;
            var reader = ReadHeaderAndSizes(inputs);
            LoadEmptyCollections();

            for (var i = 0; i < 64; i++)
            {
                var currentSize = _sizes.GetSize((TableFlag)i);
                if (currentSize > 0)
                {
                    _tables[(TableFlag)i].LoadFromMemory(ref reader, currentSize);
                    var t = _tables[(TableFlag)i].GetType().GenericTypeArguments[0];
                    _tablesByType.Add(t, _tables[(TableFlag)i]);
                }
            }

            foreach (var kv in _tables)
            {
                kv.Value.Resolve(this);
            }

            _assemblyIndexedRows = new Dictionary<(uint AssemblyTag, uint Tag), Row>();
            _indexedRows = new Dictionary<uint, Row>();
            foreach (var kv in _tables)
            {
                foreach (var r in kv.Value)
                {
                    _assemblyIndexedRows.Add((r.AssemblyTag, r.Tag), r);
                    _indexedRows.Add(r.Tag, r);
                }
            }

        }

        public Dictionary<TableFlag, int> Sizes => _sizes;
        public BlobSection Blobs => _blobs;
        public StringsSection Strings => _strings;
        public Dictionary<(uint AssemblyTag, uint Tag), Row> AssemblyIndexedRows => _assemblyIndexedRows;

        public Row GetRowByTag(uint tag)
        {
            var table = (TableFlag)(tag >> 24);
            var index = tag & 0x00FF_FFFF;
            if (table == TableFlag.UserString)
            {
                throw new NotImplementedException("User strings");
            }
            return _tables[table].GetRow((int)index);
        }

        public Span<byte> GetRVA(uint rva)
        {
            return _peFile.GetRVA(rva);
        }

        public Table<T> GetCollection<T>() where T : Row, new()
        {
            return (Table<T>)_tablesByType[typeof(T)];
        }

        private MetaDataReader ReadHeaderAndSizes(Span<byte> inputs)
        {
            inputs = inputs.Slice(4);
            inputs = inputs.Read(out _majorVersion);
            inputs = inputs.Read(out _minorVersion);
            inputs = inputs.Read(out HeapOffsetSizeFlags offsetSizes);
            inputs = inputs.Slice(1);
            inputs = inputs.Read(out ulong enabledTables);
            inputs = inputs.Read(out ulong _sortedTables);

            for (var i = 0; i < 64; i++)
            {
                var flag = 1ul << i;
                if ((flag & enabledTables) != 0)
                {
                    inputs = inputs.Read(out int size);
                    _sizes.Add((TableFlag)i, size);
                }
            }

            return new MetaDataReader(inputs, offsetSizes, _sizes);
        }
    }
}

using System;
using System.Collections.Generic;
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

        private void LoadEmptyCollections()
        {
            _tables.Add(TableFlag.Module, new Table<ModuleTableRow>(TableFlag.Module));
            _tables.Add(TableFlag.Method, new Table<MethodRow>(TableFlag.Method));
            _tables.Add(TableFlag.TypeRef, new Table<TypeRefRow>(TableFlag.TypeRef));
            _tables.Add(TableFlag.TypeDef, new Table<TypeDefRow>(TableFlag.TypeDef));
            _tables.Add(TableFlag.Field,new Table<FieldRow>(TableFlag.Field));
            _tables.Add(TableFlag.Param, new Table<ParamRow>(TableFlag.Param));
            _tables.Add(TableFlag.InterfaceImpl, new Table<InterfaceImplementationRow>(TableFlag.InterfaceImpl));
            _tables.Add(TableFlag.MemberRef, new Table<MemberRefRow>(TableFlag.MemberRef));
            _tables.Add(TableFlag.Constant, new Table<ConstantRow>(TableFlag.Constant));
            _tables.Add(TableFlag.CustomAttribute, new Table<CustomAttributeRow>(TableFlag.CustomAttribute));
            _tables.Add(TableFlag.FieldMarshal, new Table<FieldMarshalRow>(TableFlag.FieldMarshal));
            _tables.Add(TableFlag.AssemblyRef, new Table<AssemblyRefRow>(TableFlag.AssemblyRef));
            _tables.Add(TableFlag.DeclSecurity, new Table<DeclSecurityRow>(TableFlag.DeclSecurity));
            _tables.Add(TableFlag.ClassLayout, new Table<ClassLayoutRow>(TableFlag.ClassLayout));
            _tables.Add(TableFlag.FieldLayout, new Table<FieldLayoutRow>(TableFlag.FieldLayout));
            _tables.Add(TableFlag.StandAloneSig, new Table<StandAloneSigRow>(TableFlag.StandAloneSig));
            _tables.Add(TableFlag.EventMap, new Table<EventMapRow>(TableFlag.EventMap));
            _tables.Add(TableFlag.Event, new Table<EventRow>(TableFlag.Event));
            _tables.Add(TableFlag.PropertyMap, new Table<PropertyMapRow>(TableFlag.PropertyMap));
            _tables.Add(TableFlag.Property, new Table<PropertyRow>(TableFlag.Property));
            _tables.Add(TableFlag.MethodSemantics, new Table<MethodSemanticsRow>(TableFlag.MethodSemantics));
            _tables.Add(TableFlag.MethodImpl, new Table<MethodImplRow>(TableFlag.MethodImpl));
            _tables.Add(TableFlag.ModuleRef, new Table<ModuleRefRow>(TableFlag.ModuleRef));
            _tables.Add(TableFlag.TypeSpec, new Table<TypeSpecRow>(TableFlag.TypeSpec));
            _tables.Add(TableFlag.ImplMap, new Table<ImplMapRow>(TableFlag.ImplMap));
            _tables.Add(TableFlag.FieldRVA, new Table<FieldRVA>(TableFlag.FieldRVA));
            _tables.Add(TableFlag.Assembly, new Table<AssemblyRow>(TableFlag.Assembly));
            _tables.Add(TableFlag.ManifestResource, new Table<ManifestResourceRow>(TableFlag.ManifestResource));
            _tables.Add(TableFlag.NestedClass, new Table<NestedClassRow>(TableFlag.NestedClass));
            _tables.Add(TableFlag.GenericParam, new Table<GenericParamRow>(TableFlag.GenericParam));
            _tables.Add(TableFlag.MethodSpec, new Table<MethodSpecRow>(TableFlag.MethodSpec));
    }
        
        public MetaDataTables(Span<byte> inputs, StringsSection strings, BlobSection blobs)
        {
            _strings = strings;
            _blobs = blobs;
            var reader = ReadHeaderAndSizes(inputs);
            LoadEmptyCollections();

            for(var i = 0; i < 64; i++)
            {
                var currentSize = _sizes.GetSize((TableFlag)i);
                if(currentSize > 0)
                {
                    _tables[(TableFlag)i].LoadFromMemory(ref reader, currentSize);
                }
            }
           
        }

        public Dictionary<TableFlag, int> Sizes => _sizes;
        public BlobSection Blobs => _blobs;
        public StringsSection Strings => _strings;
                
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

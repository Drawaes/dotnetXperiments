using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class MetaDataTables
    {
        private Dictionary<MetadataTableFlags, int> _sizes = new Dictionary<MetadataTableFlags, int>();
        private byte _majorVersion;
        private byte _minorVersion;
        private MetadataTableFlags _sortedTables;
        private ModuleTableRow[] _modules;
        private MethodsTable _methods;
        private TypeRefTable _typesRef;
        private TypeDefTable _typeDefs;
        private FieldsTable _fieldDefs;
        private ParamsTable _params;
        private InterfaceImplementationRow[] _interfaceImpl;
        private MemberRefTable _memberRefs;
        private ConstantRow[] _constants;
        private CustomAttributeRow[] _customAttributes;
        private FieldMarshalRow[] _fieldMarshals;
        private AssemblyRefRow[] _assemblyRefs;
        private DeclSecurityRow[] _declSecurity;
        private ClassLayoutRow[] _classLayouts;
        private FieldLayoutRow[] _fieldLayouts;
        private StandAloneSig[] _standAloneSig;
        private EventMapTable _eventMaps;
        private EventTable _eventTable;
        private PropertyMapTable _propertyMap;

        public MetaDataTables(Span<byte> inputs)
        {
            var reader = ReadHeaderAndSizes(inputs);
            // 00
            _modules = ReadRowArray<ModuleTableRow>(ref reader, MetadataTableFlags.Module);
            // 01
            _typesRef = new TypeRefTable(this, ref reader);
            // 02
            _typeDefs = new TypeDefTable(this, ref reader);
            // 04
            _fieldDefs = new FieldsTable(this, ref reader);
            // 06
            _methods = new MethodsTable(this, ref reader);
            // 08
            _params = new ParamsTable(this, ref reader);
            // 09
            _interfaceImpl = ReadRowArray<InterfaceImplementationRow>(ref reader, MetadataTableFlags.InterfaceImpl);
            // 10
            _memberRefs = new MemberRefTable(this, ref reader);
            // 11
            _constants = ReadRowArray<ConstantRow>(ref reader, MetadataTableFlags.Constant);
            // 12
            _customAttributes = ReadRowArray<CustomAttributeRow>(ref reader, MetadataTableFlags.CustomAttribute);
            // 13
            _fieldMarshals = ReadRowArray<FieldMarshalRow>(ref reader, MetadataTableFlags.FieldMarshal);
            // 14
            _declSecurity = ReadRowArray<DeclSecurityRow>(ref reader, MetadataTableFlags.DeclSecurity);
            // 15
            _classLayouts = ReadRowArray<ClassLayoutRow>(ref reader, MetadataTableFlags.ClassLayout);
            // 16
            _fieldLayouts = ReadRowArray<FieldLayoutRow>(ref reader, MetadataTableFlags.FieldLayout);
            // 17
            _standAloneSig = ReadRowArray<StandAloneSig>(ref reader, MetadataTableFlags.StandAloneSig);
            // 18
            _eventMaps = new EventMapTable(this, ref reader);
            // 20
            _eventTable = new EventTable(this, ref reader);
            // 21
            _propertyMap = new PropertyMapTable(this, ref reader);
            // 23
            new PropertyTable(this, ref reader);
            // 24

            // 25

            // 26

            // 27

            // 28

            // 29

            // 32

            // 33

            // 34

            // 35
            _assemblyRefs = ReadRowArray<AssemblyRefRow>(ref reader, MetadataTableFlags.AssemblyRef);
            // 36

            // 37

            // 38

            // 39

            // 40

            // 41

            // 42

            // 44

            if (reader.Length > 0)
            {
                throw new NotImplementedException();
            }
        }

        public Dictionary<MetadataTableFlags, int> Sizes => _sizes;
        public MethodsTable Methods => _methods;

        private T[] ReadRowArray<T>(ref MetaDataReader reader, MetadataTableFlags flag)
            where T : struct, IRow
        {
            var array = new T[_sizes.GetSize(flag)];
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = new T();
                array[i].Read(ref reader);
            }
            return array;
        }

        private MetaDataReader ReadHeaderAndSizes(Span<byte> inputs)
        {
            inputs = inputs.Slice(4);
            inputs = inputs.Read(out _majorVersion);
            inputs = inputs.Read(out _minorVersion);
            inputs = inputs.Read(out HeapOffsetSizeFlags offsetSizes);
            inputs = inputs.Slice(1);
            inputs = inputs.Read(out MetadataTableFlags enabledTables);
            inputs = inputs.Read(out _sortedTables);

            for (var i = 0; i < 64; i++)
            {
                var flag = 1ul << i;
                if ((flag & (ulong)enabledTables) != 0)
                {
                    inputs = inputs.Read(out int size);
                    _sizes.Add((MetadataTableFlags)flag, size);
                }
            }

            return new MetaDataReader(inputs, offsetSizes, _sizes);
        }
    }
}

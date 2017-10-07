using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Vacuum.Core.Clr.Flags;
using Vacuum.Core.Clr.Rows;

namespace Vacuum.Core.Clr
{
    public class ClrTables:IEnumerable<IClrTable>
    {
        private ClrMetaDataHeader _metadata;
        private Dictionary<TableFlag, IClrTable> _tables = new Dictionary<TableFlag, IClrTable>();
        private Dictionary<Type, IClrTable> _tablesByType = new Dictionary<Type, IClrTable>();
        private ClrData _parentData;

        public ClrTables(ClrData data, ClrReader reader)
        {
            _parentData = data;
            _metadata = reader.Read<ClrMetaDataHeader>();
            LoadAllTables(ref reader);

            var metaReader = new ClrMetaReader(reader, this);
            foreach(var t in this)
            {
                t.LoadRows(ref metaReader);
            }
        }

        private void LoadAllTables(ref ClrReader reader)
        {
            LoadTable<Module>(ref reader);
            LoadTable<TypeRef>(ref reader);
            LoadTable<TypeDef>(ref reader);
            LoadTable<Field>(ref reader);
            LoadTable<Method>(ref reader);
            LoadTable<Param>(ref reader);
            LoadTable<InterfaceImpl>(ref reader);
            LoadTable<MemberRef>(ref reader);
            LoadTable<Constant>(ref reader);
            LoadTable<CustomAttribute>(ref reader);
            LoadTable<FieldMarshal>(ref reader);
            LoadTable<DeclSecurity>(ref reader);
            LoadTable<ClassLayout>(ref reader);
            LoadTable<FieldLayout>(ref reader);
            LoadTable<StandAloneSig>(ref reader);
            LoadTable<EventMap>(ref reader);
            LoadTable<Event>(ref reader);
            LoadTable<PropertyMap>(ref reader);
            LoadTable<Property>(ref reader);
            LoadTable<MethodSemantics>(ref reader);
            LoadTable<MethodImpl>(ref reader);
            LoadTable<ModuleRef>(ref reader);
            LoadTable<TypeSpec>(ref reader);
            LoadTable<ImplMap>(ref reader);
            LoadTable<FieldRVA>(ref reader);
            LoadTable<EncLog>(ref reader);
            LoadTable<EncMap>(ref reader);
            LoadTable<Assembly>(ref reader);
            LoadTable<AssemblyProcessor>(ref reader);
            LoadTable<AssemblyOS>(ref reader);
            LoadTable<AssemblyRef>(ref reader);
            LoadTable<AssemblyRefProcessor>(ref reader);
            LoadTable<AssemblyRefOS>(ref reader);
            LoadTable<File>(ref reader);
            LoadTable<ExportedType>(ref reader);
            LoadTable<ManifestResource>(ref reader);
            LoadTable<NestedClass>(ref reader);
            LoadTable<GenericParam>(ref reader);
            LoadTable<MethodSpec>(ref reader);
            LoadTable<GenericParamConstraint>(ref reader);
            LoadTable<Document>(ref reader);
            LoadTable<MethodDebugInformation>(ref reader);
            LoadTable<LocalScope>(ref reader);
            LoadTable<LocalVariable>(ref reader);
            LoadTable<LocalConstant>(ref reader);
            LoadTable<ImportScope>(ref reader);
            LoadTable<StateMachineMethod>(ref reader);
            LoadTable<CustomDebugInformation>(ref reader);
        }

        internal IClrTable GetTable(TableFlag flag) => _tables[flag];

        internal void Resolve()
        {
            foreach (var t in this)
            {
                t.Resolve(_parentData);
            }
        }

        internal ClrTable<T> GetTable<T>() where T : Row, new()
        {
            var table = _tablesByType[typeof(T)];
            return (ClrTable<T>)table;
        }

        public IEnumerator<IClrTable> GetEnumerator() => new Iterator(_tables);

        IEnumerator IEnumerable.GetEnumerator() => new Iterator(_tables);
        
        private void LoadTable<T>(ref ClrReader reader) where T : Row, new()
        {
            var tab = new ClrTable<T>(_metadata.EnabledTables, ref reader);
            _tables.Add(tab.TableFlag, tab);
            _tablesByType.Add(typeof(T), tab);
        }

        public HeapOffsetSizeFlags HeapSizes => _metadata.HeapOffsets;

        public int GetTableSize(TableFlag flag)
        {
            if(_tables.TryGetValue(flag, out IClrTable table))
            {
                return table.Count;
            }
            return 0;
        }

        public class Iterator : IEnumerator<IClrTable>
        {
            private int _currentIndex = -1;
            private Dictionary<TableFlag, IClrTable> _tables;
            private IClrTable _table;

            public IClrTable Current => _table;

            object IEnumerator.Current => _table;

            internal Iterator(Dictionary<TableFlag, IClrTable> tables) => _tables = tables;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                _currentIndex++;
                while (!_tables.TryGetValue((TableFlag)_currentIndex, out _table)
                || _table.Count == 0)
                {
                    if (_currentIndex < 64)
                    {
                        _currentIndex++;
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }

            public void Reset()
            {
                _currentIndex = -1;
                _table = null;
            }
        }
    }
}

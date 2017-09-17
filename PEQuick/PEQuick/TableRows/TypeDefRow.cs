using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;
using PEQuick.Importer;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class TypeDefRow : Row
    {
        private uint _flags;
        private StringIndex _nameIndex;
        private StringIndex _namespaceIndex;
        private TypeDefOrRefIndex _baseType;
        private FieldIndex _firstField;
        private MethodIndex _firstMethod;
        private ModuleRow _parentModule;
        private MethodRow[] _methods;
        private FieldRow[] _fields;

        public override TableFlag Table => TableFlag.TypeDef;
        public override uint AssemblyTag => _parentModule.AssemblyTag;
        public MethodIndex StartingMethodIndex => _firstMethod;

        public override void Resolve(MetaDataTables tables)
        {
            _parentModule = tables.GetCollection<ModuleRow>()[1];
            _nameIndex.Resolve(tables);
            _namespaceIndex.Resolve(tables);
            _baseType.Resolve(tables);

            var nextClass = tables.GetCollection<TypeDefRow>()[Index + 1];
            _methods = tables.GetCollection<MethodRow>().GetRange(_firstMethod.Index, nextClass?._firstMethod?.Index ?? int.MaxValue);
            foreach (var m in _methods)
            {
                m.Parent = this;
            }

            _fields = tables.GetCollection<FieldRow>().GetRange(_firstField.Index, nextClass?._firstField?.Index ?? int.MaxValue);
            foreach (var f in _fields)
            {
                f.Parent = this;
            }
        }

        public override void Read(ref MetaDataReader reader)
        {
            _flags = reader.Read<uint>();
            _nameIndex = reader.ReadIndex<StringIndex>();
            _namespaceIndex = reader.ReadIndex<StringIndex>();
            _baseType = reader.ReadIndex<TypeDefOrRefIndex>();
            _firstField = reader.ReadIndex<FieldIndex>();
            _firstMethod = reader.ReadIndex<MethodIndex>();
        }

        public override void GetDependencies(DependencyGather tagQueue)
        {
            tagQueue.SeedTag(_baseType.Row);
            tagQueue.SeedTags(_methods);
            tagQueue.SeedTags(_fields);
        }

        public override void WriteRow(ref MetaDataWriter writer, Dictionary<uint, uint> tokenRemapping)
        {
            throw new NotImplementedException();
        }
    }
}

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
        
        public override void GetDependencies(DependencyGather tagQueue)
        {
            tagQueue.SeedTag(_baseType.Row);
            tagQueue.SeedTags(_methods);
            tagQueue.SeedTags(_fields);
        }

        public override void WriteRow(ref MetaDataWriter writer, Dictionary<uint, uint> tokenRemapping)
        {
            writer.Write(_flags);
            writer.WriteIndex(_nameIndex);
            writer.WriteIndex(_namespaceIndex);
            writer.WriteIndex(_baseType);
            writer.WriteIndex(_firstField);
            writer.WriteIndex(_firstMethod);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;
using PEQuick.Importer;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class FieldRow : Row
    {
        

        public override uint AssemblyTag => Parent.AssemblyTag;
        
        public TypeDefRow Parent { get; internal set; }

        public override void Resolve(MetaDataTables tables)
        {
            _nameIndex.Resolve(tables);
            _signature.Resolve(tables);
        }
        
        public override string ToString() => $"{Table} - {_nameIndex.Value}";

        public override void GetDependencies(DependencyGather tagQueue)
        {
            tagQueue.SeedTag(Parent);
            //Todo Read Signature header to make sure we get dependent types
        }

        public override void WriteRow(ref MetaDataWriter writer, Dictionary<uint, uint> tokenRemapping)
        {
            writer.Write(_flags);
            writer.WriteIndex(_nameIndex);
            writer.WriteIndex(_signature);
        }
    }
}

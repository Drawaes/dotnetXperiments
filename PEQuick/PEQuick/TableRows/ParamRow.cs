using System;
using System.Collections.Generic;
using PEQuick.Flags;
using PEQuick.Importer;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class ParamRow : Row
    {
        
        
        public int Sequence => _sequence;
        public override TableFlag Table => TableFlag.Param;
        public override uint AssemblyTag => Parent.AssemblyTag;

        public MethodRow Parent { get; internal set; }

        public override void Resolve(MetaDataTables tables)
        {
            _nameIndex.Resolve(tables);
        }

        public override void Read(ref MetaDataReader reader)
        {
           
        }

        public override void GetDependencies(DependencyGather tagQueue)
        {
            tagQueue.SeedTag(Parent);
            //Todo where is the "type" that the parm is?
        }

        public override void WriteRow(ref MetaDataWriter writer, Dictionary<uint, uint> tokenRemapping)
        {
            writer.Write(_flags);
            writer.Write(_sequence);
            writer.WriteIndex(_nameIndex);
        }
    }
}
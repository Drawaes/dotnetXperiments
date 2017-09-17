﻿using System;
using PEQuick.Importer;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class ParamRow : Row
    {
        private ushort _flags;
        private ushort _sequence;
        private StringIndex _nameIndex;
        
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
            _flags = reader.Read<ushort>();
            _sequence = reader.Read<ushort>();
            _nameIndex = reader.ReadIndex<StringIndex>();
        }

        public override void GetDependencies(DependencyGather tagQueue)
        {
            tagQueue.SeedTag(Parent.Tag);
            //Todo where is the "type" that the parm is?
        }
    }
}
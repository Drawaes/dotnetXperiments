using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class ManifestResourceRow : Row
    {
        private uint _offset;
        private uint _flags;
        private StringIndex _name;
        private ImplementationIndex _implementation;

        public override void Read(ref MetaDataReader reader)
        {
            _offset = reader.Read<uint>();
            _flags = reader.Read<uint>();
            _name = reader.ReadIndex<StringIndex>();
            _implementation = reader.ReadIndex<ImplementationIndex>();
        }
    }
}

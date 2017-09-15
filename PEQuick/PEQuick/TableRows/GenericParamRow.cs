using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class GenericParamRow : Row
    {
        private ushort _number;
        private ushort _flags;
        private TypeDefOrRefIndex _owner;
        private StringIndex _name;

        public override void Read(ref MetaDataReader reader)
        {
            _number = reader.Read<ushort>();
            _flags = reader.Read<ushort>();
            _owner = reader.ReadIndex<TypeDefOrRefIndex>();
            _name = reader.ReadIndex<StringIndex>();
        }
    }
}

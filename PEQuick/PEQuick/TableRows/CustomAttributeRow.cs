using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class CustomAttributeRow : Row
    {
        public override uint AssemblyTag => _parent.Row.AssemblyTag;

        public override void Resolve(MetaDataTables tables)
        {
            _parent.Resolve(tables);
            _type.Resolve(tables);
            _value.Resolve(tables);
        }
        
        public override void WriteRow(ref MetaDataWriter writer, Dictionary<uint, uint> tokenRemapping)
        {
            writer.WriteIndex(_parent);
            writer.WriteIndex(_type);
            writer.WriteIndex(_value);
        }
    }
}

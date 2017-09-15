using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class ModuleRefRow : Row
    {
        private StringIndex _nameIndex;

        public override void Read(ref MetaDataReader reader)
        {
            _nameIndex = reader.ReadIndex<StringIndex>();
        }
    }
}

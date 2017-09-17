using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class UserStringRow : Row
    {
        private AssemblyRow _parentAssembly;

        public string Value { get; set; }
        public override TableFlag Table => TableFlag.UserString;

        public override uint AssemblyTag => _parentAssembly.AssemblyTag;

        public override void Read(ref MetaDataReader reader)
        {
        }

        public override void Resolve(MetaDataTables tables)
        {
            _parentAssembly = tables.GetCollection<AssemblyRow>()[1];
        }
    }
}

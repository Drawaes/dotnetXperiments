using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Importer;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class UserStringRow : Row
    {
        private AssemblyRow _parentAssembly;

        public UserStringRow(uint index, string s, byte token)
        {
            Value = s;
            Token = token;
        }

        public byte Token { get; set; }
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

        public override void GetDependencies(DependencyGather tagQueue)
        {
            //Nothing to do
        }
    }
}

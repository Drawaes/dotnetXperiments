using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;
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
            Index = (int)index;
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

        public override int GetHashCode()
        {
            if(Value == null)
            {
                return 0;
            }
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(obj is UserStringRow stringRow)
            {
                return stringRow.Value == Value;
            }
            return false;
        }

        public override void WriteRow(ref MetaDataWriter writer, Dictionary<uint, uint> tokenRemapping)
        {
            throw new NotImplementedException();
        }
    }
}

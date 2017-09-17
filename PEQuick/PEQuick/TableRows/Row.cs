using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Importer;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public abstract class Row
    {
        private int _index;

        public abstract TableFlag Table { get; }
        public uint Tag => (uint)(((byte)Table << 24) + _index);
        public abstract void Read(ref MetaDataReader reader);
        public abstract void Resolve(MetaDataTables tables);

        public int Index { get => _index; set => _index = value; }

        public abstract uint AssemblyTag { get; }

        public virtual void GetDependencies(DependencyGather tagQueue)
        {
            throw new NotImplementedException();
        }
    }
}

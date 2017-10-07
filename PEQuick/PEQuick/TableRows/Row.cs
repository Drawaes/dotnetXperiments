using System;
using System.Collections.Generic;
using PEQuick.Flags;
using PEQuick.Importer;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public abstract class Row
    {
        
        public abstract void Resolve(MetaDataTables tables);
                
        public abstract uint AssemblyTag { get; }

        public virtual void GetDependencies(DependencyGather tagQueue)
        {
            throw new NotImplementedException();
        }

        public virtual void UpdateRowIndex(int newIndex)
        {
            _index = newIndex;
        }

        public abstract void WriteRow(ref MetaDataWriter writer, Dictionary<uint, uint> tokenRemapping);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public interface ITable : IEnumerable<Row>
    {
        TableFlag TableFlag { get; }
        void LoadFromMemory(ref MetaDataReader reader, int size);
        void Resolve(MetaDataTables metaDataTables);
        Row GetRow(int index);
        void AddRow(Row newRow);
        int Count { get; }
        void Write(ref MetaDataWriter writer, Dictionary<uint, uint> remapper);
    }
}

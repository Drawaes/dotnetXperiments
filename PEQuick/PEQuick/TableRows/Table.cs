using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class Table<T> : ITable where T : Row, new()
    {
        private List<T> _contents;
        private TableFlag _tableFlag;
        
        public Table(TableFlag tableFlag)
        {
            _tableFlag = tableFlag;
        }

        public TableFlag TableFlag => _tableFlag;

        public void LoadFromMemory(ref MetaDataReader reader, int size)
        {
            _contents = new List<T>();
            for (var i = 1; i <= size; i++)
            {
                var n = new T();
                n.Read(ref reader);
                n.Index = i;
                _contents.Add(n);
            }
        }
    }
}

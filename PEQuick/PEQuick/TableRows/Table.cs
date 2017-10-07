using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class Table<T> : ITable where T : Row, new()
    {
        private List<T> _contents = new List<T>();
        private TableFlag _tableFlag;

        public Table()
        {
            var newType = new T();
            _tableFlag = newType.Table;
        }

        public TableFlag TableFlag => _tableFlag;
        public int Count => _contents.Count;
                
        public Row GetRow(int index) => _contents[index - 1];

        public void Write(ref MetaDataWriter writer, Dictionary<uint,uint> remapper)
        {
            foreach(var item in _contents)
            {
                item.WriteRow(ref writer, remapper);
            }
        }

        public T[] GetRange(int first, int end)
        {
            if (first == end)
            {
                return new T[0];
            }

            first = first - 1;
            end = Math.Min(_contents.Count, end - 1);
            var returnArray = new T[end - first];
            for (var i = first; i < end; i++)
            {
                returnArray[i - first] = _contents[i];
            }
            return returnArray;
        }

        public void LoadFromMemory(ref MetaDataReader reader, int size)
        {
            for (var i = 0; i < size; i++)
            {
                var n = new T();
                n.Read(ref reader);
                _contents.Add(n);
                n.Index = _contents.Count;
            }
        }

        public void Resolve(MetaDataTables metaDataTables)
        {
            if (_contents == null)
            {
                return;
            }
            foreach (var c in _contents)
            {
                c.Resolve(metaDataTables);
            }
        }

        public IEnumerator<Row> GetEnumerator() => _contents.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _contents.GetEnumerator();

        public void AddRow(Row newRow)
        {
            _contents.Add((T)newRow);
            newRow.UpdateRowIndex(_contents.Count);
        }
    }
}

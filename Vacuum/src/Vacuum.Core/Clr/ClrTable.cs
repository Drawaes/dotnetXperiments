using System;
using System.Collections.Generic;
using System.Text;
using Vacuum.Core.Clr.Flags;
using Vacuum.Core.Clr.Rows;

namespace Vacuum.Core.Clr
{
    public class ClrTable<T> : IClrTable where T : Row, new()
    {
        private int _count;
        private TableFlag _tableFlag;
        private List<T> _rows;

        public ClrTable(ulong activeRows, ref ClrReader reader)
        {
            var r = new T();
            _tableFlag = r.Flag;
            var tableType = 1ul << (byte)_tableFlag;
            if ((tableType & activeRows) == 0)
            {
                return;
            }

            _count = reader.Read<int>();
            _rows = new List<T>(_count);
        }

        public int Count => _count;
        public TableFlag TableFlag => _tableFlag;

        public T this[int index]
        {
            get
            {
                if (index == 0)
                {
                    return null;
                }
                return _rows[index - 1];
            }
        }

        public Row GetRow(int rowId) => _rows[rowId - 1];

        public void LoadRows(ref ClrMetaReader reader)
        {
            for (var i = 0; i < _count; i++)
            {
                var row = new T();
                row.LoadFromReader(ref reader, i + 1);
                _rows.Add(row);
            }
        }

        public void Resolve(ClrData parentData)
        {
            foreach (var row in _rows)
            {
                row.Resolve(parentData);
            }
        }
    }
}

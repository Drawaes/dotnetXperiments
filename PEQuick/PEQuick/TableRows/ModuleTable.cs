using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class ModuleTable
    {
        private ModuleTableRow[] _rows;
        public int PointerSize => _rows.Length > ushort.MaxValue ? 4 : 2;

        public ModuleTable(int size, ref MetaDataReader input)
        {
            _rows = new ModuleTableRow[size];
            for(var i = 0; i < _rows.Length;i++)
            {
                _rows[i] = new ModuleTableRow(ref input);
            }
        }
    }
}

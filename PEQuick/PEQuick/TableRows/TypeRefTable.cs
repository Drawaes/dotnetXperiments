using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class TypeRefTable
    {
        private TypeRefRow[] _rows;

        public TypeRefTable(Dictionary<MetadataTableFlags, int> sizes, ref MetaDataReader reader)
        {
            _rows = new TypeRefRow[sizes[MetadataTableFlags.TypeRef]];
            for(var i = 0; i < _rows.Length;i++)
            {
                _rows[i] = new TypeRefRow(ref reader);
            }
        }
    }
}

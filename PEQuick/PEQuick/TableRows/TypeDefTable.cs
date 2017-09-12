using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class TypeDefTable
    {
        private TypeDefRow[] _rows;

        public TypeDefTable(int size, ref MetaDataReader reader)
        {
            _rows = new TypeDefRow[size];
            for(var i =0;i < _rows.Length;i++)
            {
                _rows[i] = new TypeDefRow(ref reader);
                if (i > 0)
                {
                    _rows[i - 1].FieldsEnd = _rows[i].Fields;
                    _rows[i - 1].MethodsEnd = _rows[i].Methods;
                }
            }
        }

        public TypeDefRow FindRowForFieldIndex(int fieldIndex)
        {
            for (var i = 0; i < _rows.Length; i++)
            {
                if (_rows[i].Fields.Index == fieldIndex)
                {
                    return _rows[i];
                }
            }
            throw new InvalidOperationException("Unable to find a typedef that owns the row");
        }
    }
}

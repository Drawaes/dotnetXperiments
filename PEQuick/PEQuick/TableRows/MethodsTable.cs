using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class MethodsTable
    {
        private MethodRow[] _rows;

        public MethodsTable(MetaDataTables tables, ref MetaDataReader reader)
        {
            _rows = new MethodRow[tables.Sizes.GetSize(MetadataTableFlags.Method)];
            for (var i = 0; i < _rows.Length; i++)
            {
                _rows[i] = new MethodRow(ref reader);
                if (i > 0)
                {
                    _rows[i - 1].ParamListEnd = _rows[i].ParamList;
                }
                if (i == (_rows.Length - 1))
                {
                    _rows[i].ParamListEnd.SetRawIndex(uint.MaxValue);
                }
            }
        }

        public MethodRow GetMethodRowForParamIndex(int paramIndex)
        {
            for (var i = 0; i < _rows.Length; i++)
            {
                if (_rows[i].ParamList.Index == paramIndex)
                {
                    return _rows[i];
                }
            }

            throw new InvalidOperationException("Bad things");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class ParamsTable
    {
        private ParamRow[] _rows;

        public ParamsTable(MetaDataTables tables, ref MetaDataReader reader)
        {
            _rows = new ParamRow[tables.Sizes.GetSize(MetadataTableFlags.Param)];

            MethodRow method = null;

            for(var i = 0; i < _rows.Length;i++)
            {
                if((i+1) == method?.ParamListEnd.Index && method?.ParamListEnd.Index != 0)
                {
                    method = null;
                }
                if(method == null)
                {
                    method = tables.Methods.GetMethodRowForParamIndex(i + 1);
                    method.AllocateParamRows(_rows.Length - i - 1);
                }

                _rows[i] = new ParamRow(ref reader);
                method.AddParam(_rows[i]);
            }
        }
    }
}

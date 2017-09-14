using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class MemberRefTable
    {
        private MemberRefRow[] _rows;

        public MemberRefTable(MetaDataTables tables, ref MetaDataReader reader)
        {
            _rows = new MemberRefRow[tables.Sizes.GetSize(MetadataTableFlags.MemberRef)];
            for(var i = 0; i < _rows.Length;i++)
            {
                _rows[i] = new MemberRefRow(ref reader);
            }
        }
    }
}

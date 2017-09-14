using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class PropertyTable
    {
        public PropertyTable(MetaDataTables tables, ref MetaDataReader reader)
        {
            if (tables.Sizes.GetSize(MetadataTableFlags.Property) > 0)
            {
                throw new NotImplementedException();
            }
        }
    }
}

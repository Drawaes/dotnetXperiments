using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class PropertyMapTable
    {
        public PropertyMapTable(MetaDataTables tables, ref MetaDataReader reader)
        {
            if (tables.Sizes.GetSize(MetadataTableFlags.PropertyMap) > 0)
            {
                throw new NotImplementedException();
            }
        }
    }
}

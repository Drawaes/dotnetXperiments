using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class StringIndex : SimpleIndex
    {
        

        
        internal override void Resolve(MetaDataTables tables)
        {
            _value = tables.Strings.GetString(_rawIndex);
        }       
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class DeclSecurityIndex : SingleIndex
    {
        public override Row Row => throw new NotImplementedException();
        
        internal override void Resolve(MetaDataTables tables)
        {
            throw new NotImplementedException();
        }
    }
}

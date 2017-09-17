using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class DeclSecurity : Index
    {

        internal override Span<byte> Write(Span<byte> input, Dictionary<uint, uint> remapper, bool largeFormat)
        {
            throw new NotImplementedException();
        }

        internal override void Resolve(MetaDataTables tables)
        {
            throw new NotImplementedException();
        }
    }
}

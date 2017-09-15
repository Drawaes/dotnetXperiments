using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class ImplementationIndex : IIndex
    {
        private uint _rawIndex;

        public void Resolve(MetaDataTables tables)
        {
            throw new NotImplementedException();
        }

        public void SetRawIndex(uint rawIndex)
        {
            _rawIndex = rawIndex;
        }

        /*
         * Implementation: 2 bits to encode tag Tag
File 0
AssemblyRef 1
ExportedType 2
         */
    }
}

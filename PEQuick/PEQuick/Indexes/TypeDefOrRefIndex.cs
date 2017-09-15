using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class TypeDefOrRefIndex : IIndex
    {
        private uint _rawIndex;

        public void Resolve(MetaDataTables tables)
        {
            throw new NotImplementedException();
        }

        public void SetRawIndex(uint rawIndex) => _rawIndex = rawIndex;

        /*
      * TypeDefOrRef: 2 bits to encode tag Tag
TypeDef 0
TypeRef 1
TypeSpec 2
*/
    }
}

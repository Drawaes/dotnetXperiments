using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class MemberRefParentIndex : IIndex
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
         * MemberRefParent: 3 bits to encode tag Tag
TypeDef 0
TypeRef 1
ModuleRef 2
MethodDef 3
TypeSpec 4
         */
    }
}

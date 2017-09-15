using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public struct ParamIndex:IIndex
    {
        private uint _rawIndex;
                
        public uint Index => _rawIndex;

        public void Resolve(MetaDataTables tables)
        {
            throw new NotImplementedException();
        }

        public void SetRawIndex(uint rawIndex) => _rawIndex = rawIndex;

        /*
         * ResolutionScope: 2 bits to encode tag Tag
Module 0
ModuleRef 1
AssemblyRef 2
TypeRef 3
         */
    }
}

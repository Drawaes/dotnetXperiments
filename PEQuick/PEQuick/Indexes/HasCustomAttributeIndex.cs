using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class HasCustomAttributeIndex : IIndex
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
         * HasCustomAttribute: 5 bits to encode tag Tag
MethodDef 0
Field 1
TypeRef 2
TypeDef 3
Param 4
InterfaceImpl 5
MemberRef 6
Module 7
Permission 8
Property 9
Event 10
StandAloneSig 11
ModuleRef 12
TypeSpec 13
Assembly 14
AssemblyRef 15
File 16
ExportedType 17
ManifestResource 18
GenericParam 19
GenericParamConstraint 20
MethodSpec 21

         */
    }
}

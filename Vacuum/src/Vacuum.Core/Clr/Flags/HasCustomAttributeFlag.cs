using System;
using System.Collections.Generic;
using System.Text;

namespace Vacuum.Core.Clr.Flags
{
    public enum HasCustomAttributeFlag
    {
        Method = 0,
        Field = 1,
        TypeRef = 2,
        TypeDef = 3,
        Param = 4,
        InterfaceImplementation = 5,
        MemberRef = 6,
        Module = 7,
        Permission = 8,
        Property = 9,
        Event = 10,
        StandAloneSig = 11,
        ModuleRef = 12,
        TypeSpec = 13,
        Assembly = 14,
        AssemblyRef = 15,
        File = 16,
        ExportedType = 17,
        ManifestResource = 18,
    }
}

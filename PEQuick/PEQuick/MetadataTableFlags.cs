using System;
using System.Collections.Generic;
using System.Text;

namespace PEQuick
{
    [Flags]
    public enum MetadataTableFlags : ulong
    {
        Module = 1,
        TypeRef = 2,
        TypeDef = 4,
        Reserved1 = 8,
        Field = 16,
        Reserved2 = 32,
        Method = 64,
        Reserved3 = 128,
        Param = 256,
        InterfaceImpl = 512,
        MemberRef = 1024,
        Constant = 2048,
        CustomAttribute = 4096,
        FieldMarshal = 8192,
        DeclSecurity = 16384,
        ClassLayout = 32768,
        FieldLayout = 65536,
        StandAloneSig = 131072,
        EventMap = 262144,
        Reserved4 = 524288,
        Event = 1048576,
        PropertyMap = 2097152,
        Reserved5 = 4194304,
        Property = 8388608,
        MethodSemantics = 16777216,
        MethodImpl = 33554432,
        ModuleRef = 67108864,
        TypeSpec = 134217728,
        ImplMap = 268435456,
        FieldRVA = 536870912,
        Reserved6 = 1073741824,
        Reserved7 = 2147483648,
        Assembly = 4294967296,
        AssemblyProcessor = 8589934592,
        AssemblyOS = 17179869184,
        AssemblyRef = 34359738368,
        AssemblyRefProcessor = 68719476736,
        AssemblyRefOS = 137438953472,
        File = 274877906944,
        ExportedType = 549755813888,
        ManifestResource = 1099511627776,
        NestedClass = 2199023255552,
        GenericParam = 4398046511104,
        MethodSpec = 8796093022208,
        GenericParamConstraint = 17592186044416,
    }
}

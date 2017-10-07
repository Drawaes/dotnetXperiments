using System;
using System.Collections.Generic;
using System.Text;

namespace PEQuick.Flags
{
    [Flags]
    public enum TableFlag : byte
    {
        FieldPtr = 0x03,
        MethodPtr = 0x05,
        ParamPtr = 0x07,
        EventPtr = 0x13,
        PropertyPtr = 0x16,
 
        StateMachineMethod = 0x36,
        CustomDebugInformation = 0x37,
        UserString = 0x70,
        Guid = 0x71,
        Blob = 0x72,
        Strings = 0x73,
    }
}

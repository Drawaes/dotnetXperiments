using System;
using System.Collections.Generic;
using System.Text;

namespace PEQuick.Flags
{
    public enum OperandType
    {
        InlineNone,
        ShortInlineVar,
        ShortInlineI,
        InlineI,
        InlineI8,
        ShortInlineR,
        InlineR,
        InlineMethod,
        InlineSig,
        ShortInlineBrTarget,
        InlineBrTarget,
        InlineSwitch,
        InlineType,
        InlineString,
        InlineField,
        InlineTok,
        InlineVar,
    }
}

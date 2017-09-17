using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;

namespace PEQuick.IL
{
    public class ILTracer
    {
        private static OperandType[] _singleByteCodes = new OperandType[256];
        private static OperandType[] _twoByteCodes = new OperandType[35];
        private static int currentOffset;

        static ILTracer()
        {
            SingleByteCodes();
            TwoByteCodes();
        }

        public static OperandType GetSingleByteOperandType(byte opType) => _singleByteCodes[opType];
        public static OperandType GetTwoByteOperandType(byte opType) => _twoByteCodes[opType];

        private static void TwoByteCodes()
        {
            currentOffset = 0;
            Add2(OperandType.InlineNone, 6);
            Add2(OperandType.InlineMethod, 2);
            Add2(OperandType.InlineNone, 1);
            Add2(OperandType.InlineVar, 6);
            Add2(OperandType.InlineNone, 3);
            Add2(OperandType.ShortInlineI, 1);
            Add2(OperandType.InlineNone, 2);
            Add2(OperandType.InlineType, 1);
            Add2(OperandType.InlineNone, 6);
            Add2(OperandType.InlineType, 1);
            Add2(OperandType.InlineNone, 6);
        }

        private static void SingleByteCodes()
        {
            Add(OperandType.InlineNone, 14);
            Add(OperandType.ShortInlineVar, 6);
            Add(OperandType.InlineNone, 11);
            Add(OperandType.ShortInlineI, 1);
            Add(OperandType.InlineI, 1);
            Add(OperandType.InlineI8, 1);
            Add(OperandType.ShortInlineR, 1);
            Add(OperandType.InlineR, 1);
            Add(OperandType.InlineNone, 3);
            Add(OperandType.InlineMethod, 2);
            Add(OperandType.InlineSig, 1);
            Add(OperandType.InlineNone, 1);
            Add(OperandType.ShortInlineBrTarget, 13);
            Add(OperandType.InlineBrTarget, 13);
            Add(OperandType.InlineSwitch, 1);
            Add(OperandType.InlineNone, 41);
            Add(OperandType.InlineMethod, 1);
            Add(OperandType.InlineType, 2);
            Add(OperandType.InlineString, 1);
            Add(OperandType.InlineMethod, 1);
            Add(OperandType.InlineType, 2);
            Add(OperandType.InlineNone, 3);
            Add(OperandType.InlineType, 1);
            Add(OperandType.InlineNone, 1);
            Add(OperandType.InlineField, 6);
            Add(OperandType.InlineType, 1);
            Add(OperandType.InlineNone, 10);
            Add(OperandType.InlineType, 2);
            Add(OperandType.InlineNone, 1);
            Add(OperandType.InlineType, 1);
            Add(OperandType.InlineNone, 50);
            Add(OperandType.InlineType, 1);
            Add(OperandType.InlineNone, 3);
            Add(OperandType.InlineType, 1);
            Add(OperandType.InlineNone, 9);
            Add(OperandType.InlineTok, 1);
            Add(OperandType.InlineNone, 12);
            Add(OperandType.InlineBrTarget, 1);
            Add(OperandType.ShortInlineBrTarget, 1);
            Add(OperandType.InlineNone, 33);
        }

        private static void Add2(OperandType type, int number)
        {
            var startOffset = currentOffset + number;
            for (; currentOffset < startOffset; currentOffset++)
            {
                _twoByteCodes[currentOffset] = type;
            }
        }

        private static void Add(OperandType type, int number)
        {
            var startOffset = currentOffset + number;
            for(; currentOffset < startOffset;currentOffset++)
            {
                _singleByteCodes[currentOffset] = type;
            }
        }
    }
}

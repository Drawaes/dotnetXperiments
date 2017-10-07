using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PEQuick.Flags;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.IL
{
    public class MethodBody
    {
        private MethodBodyFormat _format;
        private uint _size;
        private byte[] _methodBody;
        private List<(int Location, Row Row)> _tags = new List<(int Location, Row Row)>();

        public MethodBody(uint rva, MetaDataTables tables)
        {
            var methodBody = tables.GetRVA(rva);
            _format = (MethodBodyFormat)methodBody[0] & MethodBodyFormat.Mask;
            switch (_format)
            {
                case MethodBodyFormat.Tiny:
                    //TinyFormat
                    _size = (ushort)(methodBody[0] >> 2);
                    ParseIL(methodBody.Slice(1, (int)_size), tables);
                    _methodBody = methodBody.Slice(0, (int)_size + 1).ToArray();
                    break;
                case MethodBodyFormat.Fat:
                    //FatFormat
                    ParseFatBody(methodBody, tables);
                    break;
                default:
                    throw new InvalidOperationException();
            }

        }

        public Span<byte> GetBody(Dictionary<uint,uint> remapper)
        {
            //TODO remap the body as needed, for now this isn't being
            //done
            return _methodBody.AsSpan();
        }

        public IEnumerable<Row> DependentTags => _tags.Select(t => t.Row);

        private void ParseFatBody(Span<byte> input, MetaDataTables tables)
        {
            var originalSpan = input;
            input = input.Read(out ushort headerFlags);
            input = input.Read(out ushort maxStack);
            input = input.Read(out uint codeSize);
            input = input.Read(out uint localvar);
            ParseIL(input.Slice(0, (int)codeSize), tables);

            var totalBodySize = originalSpan.Length - input.Slice((int)codeSize).Length;
            _methodBody = originalSpan.Slice(0, totalBodySize).ToArray();

            //TODO exception handlers and extra sections
        }

        private void ParseIL(Span<byte> il, MetaDataTables tables)
        {
            var intialLength = il.Length;
            while (il.Length > 0)
            {
                OperandType opType;
                var currentByte = il[0];
                if (currentByte == 0xfe)
                {
                    opType = ILTracer.GetTwoByteOperandType(il[1]);
                    il = il.Slice(2);
                }
                else
                {
                    opType = ILTracer.GetSingleByteOperandType(currentByte);
                    il = il.Slice(1);
                }

                // Now we need to decide what to do based on the operand

                switch (opType)
                {
                    case OperandType.InlineBrTarget:
                    case OperandType.InlineI:
                    case OperandType.ShortInlineR:
                        il = il.Slice(4);
                        break;
                    case OperandType.ShortInlineI:
                    case OperandType.ShortInlineVar:
                    case OperandType.ShortInlineBrTarget:
                        il = il.Slice(1);
                        break;
                    case OperandType.InlineField:
                    case OperandType.InlineMethod:
                    case OperandType.InlineSig:
                    case OperandType.InlineString:
                    case OperandType.InlineTok:
                    case OperandType.InlineType:
                        var currentIndex = intialLength - il.Length;
                        il = il.Read(out uint tag);
                        _tags.Add((currentIndex, tables.GetRowByTag(tag)));
                        break;
                    case OperandType.InlineNone:
                        break;
                    case OperandType.InlineSwitch:
                        il = il.Read(out int numSwitches);
                        for (var i = 0; i < numSwitches; i++)
                        {
                            il = il.Read(out int switchTarget);
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }
    }
}

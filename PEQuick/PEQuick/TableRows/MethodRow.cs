using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class MethodRow
    {
        private uint _rva;
        private ushort _methodImplAttributes;
        private MethodAttributesFlags _flags;
        private StringIndex _name;
        private BlobIndex _signature;
        private ParamIndex _paramList;
        private ParamIndex _paramListEnd;
        private ParamRow[] _params;

        public MethodRow(ref MetaDataReader reader)
        {
            _rva = reader.Read<uint>();
            _methodImplAttributes = reader.Read<ushort>();
            _flags = reader.Read<MethodAttributesFlags>();
            _name = reader.ReadIndex<StringIndex>();
            _signature = reader.ReadIndex<BlobIndex>();
            _paramList = reader.ReadIndex<ParamIndex>();
            _paramListEnd = new ParamIndex();
        }

        internal void AllocateParamRows(int v)
        {
            var numberOfRows = _paramListEnd.Index == 0 ? v : (int)_paramListEnd.Index - _paramList.Index;
            _params = new ParamRow[numberOfRows];
        }

        internal void AddParam(ParamRow paramRow)
        {
            _params[paramRow.Sequence-1] = paramRow;
        }

        public ParamIndex ParamList => _paramList;
        public ParamIndex ParamListEnd { get => _paramListEnd; set => _paramListEnd = value; }
    }
}

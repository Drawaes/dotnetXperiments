using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class MethodRow : Row
    {
        private uint _rva;
        private ushort _methodImplAttributes;
        private MethodAttributesFlags _flags;
        private StringIndex _nameIndex;
        private BlobIndex _signature;
        private ParamIndex _firstParam;
        private byte[] _methodBody;
        private ParamRow[] _params;

        public override TableFlag Table => TableFlag.Method;
        public override uint AssemblyTag => Parent.AssemblyTag;
        public TypeDefRow Parent { get; set; }

        public override void Resolve(MetaDataTables tables)
        {
            _nameIndex.Resolve(tables);
            _signature.Resolve(tables);

            var nextMethod = tables.GetCollection<MethodRow>()[Index + 1];
            _params = tables.GetCollection<ParamRow>().GetRange((int)_firstParam.Index, (int)(nextMethod?._firstParam?.Index ?? int.MaxValue));
            foreach (var f in _params)
            {
                f.Parent = this;
            }

            ResolveMethodBody(tables);
        }

        private void ResolveMethodBody(MetaDataTables tables)
        {
            if (_rva == 0)
            {
                return;
            }
            var methodBody = tables.GetRVA(_rva);
            var methodFormat = methodBody[0] & 0x03;
            ushort size;
            switch (methodFormat)
            {
                case 0x02:
                    //TinyFormat
                    size = (ushort)(methodBody[0] >> 2);
                    break;
                case 0x03:
                    //FatFormat
                    methodBody.Slice(2).Read(out size);
                    size++;
                    break;
                default:
                    throw new InvalidOperationException();
            }
            _methodBody = methodBody.Slice(0, (size + 1)).ToArray();
        }

        public override void Read(ref MetaDataReader reader)
        {
            _rva = reader.Read<uint>();
            _methodImplAttributes = reader.Read<ushort>();
            _flags = reader.Read<MethodAttributesFlags>();
            _nameIndex = reader.ReadIndex<StringIndex>();
            _signature = reader.ReadIndex<BlobIndex>();
            _firstParam = reader.ReadIndex<ParamIndex>();
        }

        public byte[] Signature => _signature.Value;
        public byte[] MethodBody => _methodBody;
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;
using PEQuick.Importer;
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
        public string Name => _nameIndex.Value;
        public Span<byte> Signature => _signature.Value.AsSpan();
        public byte[] MethodBody => _methodBody;

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
        
        public override void Read(ref MetaDataReader reader)
        {
            _rva = reader.Read<uint>();
            _methodImplAttributes = reader.Read<ushort>();
            _flags = reader.Read<MethodAttributesFlags>();
            _nameIndex = reader.ReadIndex<StringIndex>();
            _signature = reader.ReadIndex<BlobIndex>();
            _firstParam = reader.ReadIndex<ParamIndex>();
        }

        public override void GetDependencies(DependencyGather tagQueue)
        {
            tagQueue.SeedTag(Parent.Tag);
            tagQueue.SeedTags(_params);
            //TODO parse body
        }

        private void ParseBody()
        {
            var methodFormat = (MethodBodyFormat) MethodBody[0] & MethodBodyFormat.Mask;
            switch(methodFormat)
            {
                case MethodBodyFormat.Fat:
                    throw new NotImplementedException();
                case MethodBodyFormat.Tiny:
                    throw new NotImplementedException();
            }
        }

        private void ParseTinyBody()
        {
            var size = MethodBody[0] >> 2;

        }
    }
}

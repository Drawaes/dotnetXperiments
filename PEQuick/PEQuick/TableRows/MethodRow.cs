using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;
using PEQuick.IL;
using PEQuick.Importer;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class MethodRow : Row
    {
        
        private MethodBody _methodBody;
        private ParamRow[] _params;

        public override TableFlag Table => TableFlag.Method;
        public override uint AssemblyTag => Parent.AssemblyTag;
        public TypeDefRow Parent { get; set; }
        public string Name => _nameIndex.Value;
        public Span<byte> Signature => _signature.Value.AsSpan();
        
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

            if(_rva != 0)
            {
                _methodBody = new MethodBody(_rva, tables);
            }
        }


        public override void GetDependencies(DependencyGather tagQueue)
        {
            tagQueue.SeedTag(Parent);
            tagQueue.SeedTags(_params);
            if (_methodBody != null)
            {
                tagQueue.SeedTags(_methodBody.DependentTags);
            }
            //TODO parse body
        }

        public override void WriteRow(ref MetaDataWriter writer, Dictionary<uint, uint> tokenRemapping)
        {
            uint index;
            if(_methodBody == null)
            {
                index = 0;
            }
            else
            {
                index = (uint) writer.WriteRVABlob(_methodBody.GetBody(tokenRemapping));
            }
            writer.Write(index);

            writer.Write(_methodImplAttributes);
            writer.Write(_flags);
            writer.WriteIndex(_nameIndex);
            writer.WriteIndex(_signature);
            writer.WriteIndex(_firstParam);
        }
    }
}

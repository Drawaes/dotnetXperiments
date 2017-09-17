using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Flags;
using PEQuick.Importer;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class TypeSpecRow : Row
    {
        private BlobIndex _signature;
        private TypeDefOrRefIndex _parentRow;

        public override TableFlag Table => TableFlag.TypeSpec;
        public override uint AssemblyTag => _parentRow.Row.AssemblyTag;

        public override void Resolve(MetaDataTables tables)
        {
            _signature.Resolve(tables);
            var elementType = (ElementType)_signature.Value[0];
            switch(elementType)
            {
                case ElementType.ELEMENT_TYPE_GENERICINST:
                    ProcessGenericInst(tables);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void ProcessGenericInst(MetaDataTables tables)
        {
            var span = _signature.Value.AsSpan<byte>();
            var elementType = (ElementType)span[1];
            span = span.Slice(2);
            switch(elementType)
            {
                case ElementType.ELEMENT_TYPE_CLASS:
                    span = span.ReadEncodedInt(out uint output);
                    _parentRow = new TypeDefOrRefIndex();
                    _parentRow.SetRawIndex(output);
                    _parentRow.Resolve(tables);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public override void Read(ref MetaDataReader reader)
        {
            _signature = reader.ReadIndex<BlobIndex>();
        }

        public override void GetDependencies(DependencyGather tagQueue)
        {
            tagQueue.SeedTag(_parentRow.Row);
            //TODO check for arrays etc
        }
    }
}

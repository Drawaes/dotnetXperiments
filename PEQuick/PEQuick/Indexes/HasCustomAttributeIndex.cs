using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class HasCustomAttributeIndex : MultiIndex
    {
        private Row _row;

        protected override byte BitMask => 0b0001_1111;
        protected override byte BitShift => 5;
        public override Row Row => _row;
        
        internal override void Resolve(MetaDataTables tables)
        {
            var flag = _rawIndex & BitMask;
            var index = (int)(_rawIndex >> BitShift);
            switch (flag)
            {
                case 0:
                    _row = tables.GetCollection<MethodRow>()[index];
                    break;
                case 1:
                    _row = tables.GetCollection<FieldRow>()[index];
                    break;
                case 2:
                    _row = tables.GetCollection<TypeRefRow>()[index];
                    break;
                case 3:
                    _row = tables.GetCollection<TypeDefRow>()[index];
                    break;
                case 4:
                    _row = tables.GetCollection<ParamRow>()[index];
                    break;
                case 5:
                    _row = tables.GetCollection<InterfaceImplementationRow>()[index];
                    break;
                case 6:
                    _row = tables.GetCollection<MemberRefRow>()[index];
                    break;
                case 7:
                    _row = tables.GetCollection<ModuleRow>()[index];
                    break;
                case 8:
                    throw new NotImplementedException();
                case 9:
                    _row = tables.GetCollection<PropertyRow>()[index];
                    break;
                case 10:
                    _row = tables.GetCollection<EventRow>()[index];
                    break;
                case 11:
                    _row = tables.GetCollection<StandAloneSigRow>()[index];
                    break;
                case 12:
                    _row = tables.GetCollection<ModuleRefRow>()[index];
                    break;
                case 13:
                    _row = tables.GetCollection<TypeSpecRow>()[index];
                    break;
                case 14:
                    _row = tables.GetCollection<AssemblyRow>()[index];
                    break;
                case 15:
                    _row = tables.GetCollection<AssemblyRefRow>()[index];
                    break;
                case 16:
                    //File type
                    throw new NotImplementedException();
                case 17:
                    //Exported Type
                    throw new NotImplementedException();
                case 18:
                    _row = tables.GetCollection<ManifestResourceRow>()[index];
                    break;
                case 19:
                    _row = tables.GetCollection<GenericParamRow>()[index];
                    break;
                case 20:
                    //Generic Param Constraint
                    throw new NotImplementedException();
                case 21:
                    _row = tables.GetCollection<MethodSpecRow>()[index];
                    break;
            }
        }
    }
}

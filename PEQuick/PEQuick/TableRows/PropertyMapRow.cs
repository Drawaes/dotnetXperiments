using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class PropertyMapRow : Row
    {
        private TypeDefIndex _parent;
        private PropertyIndex _propertyList;
        private PropertyRow[] _properties;

        public override TableFlag Table => TableFlag.PropertyMap;
        public override uint AssemblyTag => _parent.Row.AssemblyTag;

        public override void Resolve(MetaDataTables tables)
        {
            _parent.Resolve(tables);
            _propertyList.Resolve(tables);

            var nextPropertyMap = tables.GetCollection<PropertyMapRow>()[Index + 1];
            _properties = tables.GetCollection<PropertyRow>().GetRange(_propertyList.Index, nextPropertyMap?._propertyList?.Index ?? int.MaxValue);
            foreach(var p in _properties)
            {
                p.Parent = this;
            }
        }

        public override void Read(ref MetaDataReader reader)
        {
            _parent = reader.ReadIndex<TypeDefIndex>();
            _propertyList = reader.ReadIndex<PropertyIndex>();
        }
    }
}

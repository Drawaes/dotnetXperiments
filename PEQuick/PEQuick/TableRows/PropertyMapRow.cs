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

        public override void Read(ref MetaDataReader reader)
        {
            _parent = reader.ReadIndex<TypeDefIndex>();
            _propertyList = reader.ReadIndex<PropertyIndex>();
        }
    }
}

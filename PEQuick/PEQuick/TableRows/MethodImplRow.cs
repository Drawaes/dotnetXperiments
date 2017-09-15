using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class MethodImplRow : Row
    {
        private TypeDefIndex _class;
        private MethodDefOrRefIndex _methodBody;
        private MethodDefOrRefIndex _methodDeclaration;

        public override void Read(ref MetaDataReader reader)
        {
            _class = reader.ReadIndex<TypeDefIndex>();
            _methodBody = reader.ReadIndex<MethodDefOrRefIndex>();
            _methodDeclaration = reader.ReadIndex<MethodDefOrRefIndex>();

        }
    }
}

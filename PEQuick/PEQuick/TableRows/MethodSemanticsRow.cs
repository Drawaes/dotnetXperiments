using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class MethodSemanticsRow : Row
    {
        private ushort _semantics;
        private MethodIndex _method;
        private HasSemanticsIndex _association;

        public override void Read(ref MetaDataReader reader)
        {
            _semantics = reader.Read<ushort>();
            _method = reader.ReadIndex<MethodIndex>();
            _association = reader.ReadIndex<HasSemanticsIndex>();
        }
    }
}

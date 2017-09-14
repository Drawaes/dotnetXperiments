using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public interface IRow
    {
        void Read(ref MetaDataReader reader);
    }
}

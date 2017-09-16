using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Indexes
{
    public class BlobIndex : Index
    {
        private byte[] _content;
        private bool _resolved;

        public int Index => (int)_rawIndex;
        
        internal override void Resolve(MetaDataTables tables)
        {
            _resolved = true;
            if (_rawIndex == 0)
            {
                return;
            }
            _content = tables.Blobs.GetBlob(_rawIndex);
        }
                
        public byte[] Value
        {
            get
            {
                if(!_resolved)
                {
                    throw new InvalidOperationException();
                }
                return _content;
            }
        }
    }
}

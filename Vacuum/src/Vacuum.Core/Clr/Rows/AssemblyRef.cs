using System;
using System.Collections.Generic;
using System.Text;
using Vacuum.Core.Clr.Flags;
using Vacuum.Core.Clr.Indexes;

namespace Vacuum.Core.Clr.Rows
{
    public class AssemblyRef : Row
    {
        private ushort _majorVersion;
        private ushort _minorVersion;
        private ushort _buildNumber;
        private ushort _revisionNumber;
        private uint _flags;
        private BlobIndex _publicKeyOrToken;
        private StringIndex _name;
        private StringIndex _cultureIndex;
        private BlobIndex _hashValue;

        public AssemblyRef()
        {
        }
        
        public override TableFlag Flag => TableFlag.AssemblyRef;

        internal override void LoadFromReader(ref ClrMetaReader reader, int index)
        {
            _index = index;
            _majorVersion = reader.Read<ushort>();
            _minorVersion = reader.Read<ushort>();
            _buildNumber = reader.Read<ushort>();
            _revisionNumber = reader.Read<ushort>();
            _flags = reader.Read<uint>();
            _publicKeyOrToken = reader.ReadIndex<BlobIndex>();
            _name = reader.ReadIndex<StringIndex>();
            _cultureIndex = reader.ReadIndex<StringIndex>();
            _hashValue = reader.ReadIndex<BlobIndex>();
        }

        internal override void Resolve(ClrData clrData)
        {
            throw new NotImplementedException();
        }
    }
}

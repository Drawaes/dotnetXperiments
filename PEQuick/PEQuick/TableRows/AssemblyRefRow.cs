using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class AssemblyRefRow : Row
    {
        private ushort _majorVersion;
        private ushort _minorVersion;
        private ushort _buildNumber;
        private ushort _revisionNumber;
        private uint _flags;
        private BlobIndex _publicKeyOrToken;
        private StringIndex _nameIndex;
        private StringIndex _cultureIndex;
        private BlobIndex _hashValue;

        public override void Read(ref MetaDataReader reader)
        {
            _majorVersion = reader.Read<ushort>();
            _minorVersion = reader.Read<ushort>();
            _buildNumber = reader.Read<ushort>();
            _revisionNumber = reader.Read<ushort>();
            _flags = reader.Read<uint>();
            _publicKeyOrToken = reader.ReadIndex<BlobIndex>();
            _nameIndex = reader.ReadIndex<StringIndex>();
            _cultureIndex = reader.ReadIndex<StringIndex>();
            _hashValue = reader.ReadIndex<BlobIndex>();
        }

    }
}

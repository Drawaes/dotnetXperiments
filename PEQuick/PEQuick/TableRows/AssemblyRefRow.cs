using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public struct AssemblyRefRow : IRow
    {
        private ushort _majorVersion;
        private ushort _minorVersion;
        private ushort _buildNumber;
        private ushort _revisionNumber;
        private uint _flags;
        private BlobIndex _publicKeyOrToken;
        private StringIndex _name;
        private StringIndex _culture;
        private BlobIndex _hashValue;

        public void Read(ref MetaDataReader reader)
        {
            _majorVersion = reader.Read<ushort>();
            _minorVersion = reader.Read<ushort>();
            _buildNumber = reader.Read<ushort>();
            _revisionNumber = reader.Read<ushort>();
            _flags = reader.Read<uint>();
            _publicKeyOrToken = reader.ReadIndex<BlobIndex>();
            _name = reader.ReadIndex<StringIndex>();
            _culture = reader.ReadIndex<StringIndex>();
            _hashValue = reader.ReadIndex<BlobIndex>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class AssemblyRow : Row
    {
        public uint HashAlgId { get; set; }
        public ushort MajorVersion { get; set; }
        public ushort MinorVersion { get; set; }
        public ushort BuildNumber { get; set; }
        public ushort RevisionNumber { get; set; }
        public uint Flags { get; set; }
        public BlobIndex PublicKey { get; set; }
        public StringIndex _nameIndex;
        public StringIndex _cultureIndex;

        public override void Read(ref MetaDataReader reader)
        {
            HashAlgId = reader.Read<uint>();
            MajorVersion = reader.Read<ushort>();
            MinorVersion = reader.Read<ushort>();
            BuildNumber = reader.Read<ushort>();
            RevisionNumber = reader.Read<ushort>();
            Flags = reader.Read<uint>();
            PublicKey = reader.ReadIndex<BlobIndex>();
            _nameIndex = reader.ReadIndex<StringIndex>();
            _cultureIndex = reader.ReadIndex<StringIndex>();
        }
    }
}

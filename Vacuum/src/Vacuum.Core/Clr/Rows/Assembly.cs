using System;
using System.Collections.Generic;
using System.Text;
using Vacuum.Core.Clr.Flags;
using Vacuum.Core.Clr.Indexes;

namespace Vacuum.Core.Clr.Rows
{
    public class Assembly : Row
    {
        public uint HashAlgId { get; set; }
        public ushort MajorVersion { get; set; }
        public ushort MinorVersion { get; set; }
        public ushort BuildNumber { get; set; }
        public ushort RevisionNumber { get; set; }
        public uint Flags { get; set; }
        public BlobIndex PublicKey { get; set; }
        public StringIndex Name { get; set; }
        public StringIndex CultureIndex { get; set; }

        public Assembly()
        {
        }
                
        public override TableFlag Flag => TableFlag.Assembly;

        internal override void LoadFromReader(ref ClrMetaReader reader, int index)
        {
            _index = index;
            HashAlgId = reader.Read<uint>();
            MajorVersion = reader.Read<ushort>();
            MinorVersion = reader.Read<ushort>();
            BuildNumber = reader.Read<ushort>();
            RevisionNumber = reader.Read<ushort>();
            Flags = reader.Read<uint>();
            PublicKey = reader.ReadIndex<BlobIndex>();
            Name = reader.ReadIndex<StringIndex>();
            CultureIndex = reader.ReadIndex<StringIndex>();
        }

        internal override void Resolve(ClrData clrData)
        {
            throw new NotImplementedException();
        }
    }
}

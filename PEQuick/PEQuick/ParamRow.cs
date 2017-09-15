using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class ParamRow : Row
    {
        private ushort _flags;
        private ushort _sequence;
        private StringIndex _nameIndex;

        public int Sequence => _sequence;

        public override void Read(ref MetaDataReader reader)
        {
            _flags = reader.Read<ushort>();
            _sequence = reader.Read<ushort>();
            _nameIndex = reader.ReadIndex<StringIndex>();
        }
    }
}
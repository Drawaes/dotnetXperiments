using PEQuick.Indexes;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class ParamRow
    {
        private ushort _flags;
        private ushort _sequence;
        private StringIndex _name;

        public ParamRow(ref MetaDataReader reader)
        {
            _flags = reader.Read<ushort>();
            _sequence = reader.Read<ushort>();
            _name = reader.ReadIndex<StringIndex>();
        }

        public int Sequence => _sequence;
    }
}
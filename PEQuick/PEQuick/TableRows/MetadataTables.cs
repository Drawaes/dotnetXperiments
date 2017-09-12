using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public class MetaDataTables
    {
        private Dictionary<MetadataTableFlags, int> _sizes = new Dictionary<MetadataTableFlags, int>();
        private byte _majorVersion;
        private byte _minorVersion;
        private HeapOffsetSizeFlags _offsetSizes;
        private MetadataTableFlags _enabledTables;
        private MetadataTableFlags _sortedTables;

        public MetaDataTables(Span<byte> inputs)
        {
            inputs = inputs.Slice(4);
            inputs = inputs.Read(out _majorVersion);
            inputs = inputs.Read(out _minorVersion);
            inputs = inputs.Read(out _offsetSizes);
            inputs = inputs.Slice(1);
            inputs = inputs.Read(out _enabledTables);
            inputs = inputs.Read(out _sortedTables);

            for (var i = 0; i < 64; i++)
            {
                var flag = 1ul << i;
                if ((flag & (ulong)_enabledTables) != 0)
                {
                    inputs = inputs.Read(out int size);
                    _sizes.Add((MetadataTableFlags)flag, size);
                }
            }

            var reader = new MetaDataReader(inputs, _offsetSizes, _sizes);

            if((_enabledTables & MetadataTableFlags.Module) != 0)
            {
                var moduleTable = new ModuleTable(_sizes[MetadataTableFlags.Module], ref reader);
            }

            if((_enabledTables & MetadataTableFlags.TypeRef) != 0)
            {
                var typeDefs = new TypeRefTable(_sizes, ref reader);
            }
        }
    }
}

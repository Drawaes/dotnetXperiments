using System;
using System.Collections.Generic;
using System.Text;
using Vacuum.Core.Clr.Flags;
using Vacuum.Core.Clr.Indexes;

namespace Vacuum.Core.Clr
{
    public struct ClrMetaReader
    {
        private ClrReader _reader;
        private Dictionary<Type, bool> _bigIndexes;

        public ClrMetaReader(ClrReader reader, ClrTables tables)
        {
            _reader = reader;
            _bigIndexes = IndexSizes.GetLargeSizes(flag => tables.GetTableSize(flag), tables.HeapSizes);
        }

        public T Read<T>() where T : struct => _reader.Read<T>();

        public T ReadIndex<T>() where T : Index, new()
        {
            var useBigIndex = _bigIndexes[typeof(T)];
            uint rawIndex;
            if (useBigIndex)
            {
                rawIndex = _reader.Read<uint>();
            }
            else
            {
                rawIndex = _reader.Read<ushort>();
            }
            var returnIndex = new T();
            returnIndex.SetRawIndex(rawIndex);
            return returnIndex;
            
        }

        
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Vacuum.Core.Clr.Flags;
using Vacuum.Core.Clr.Rows;
using Vacuum.Core.PE;

namespace Vacuum.Core.Clr
{
    public class ClrData
    {
        private ClrHeader _header;
        private ClrDataHeader _dataHeader;
        private ImageDataDirectory _clrDirectory;
        private StreamHeader[] _streamHeaders;
        private PEFileReader _peFile;
        private ClrStrings _strings;
        private ClrUserStrings _userStrings;
        private ClrGuids _guids;
        private ClrTables _tables;
        
        public ClrData(ImageDataDirectory directory, PEFileReader peFile)
        {
            _peFile = peFile;
            _clrDirectory = directory;
            var span = peFile.GetImageDirectory(_clrDirectory);
            span = span.Read(out _header);

            span = peFile.GetImageDirectory(_header.MetaData);

            var reader = new ClrReader(span);
            _dataHeader = new ClrDataHeader(ref reader);
            _streamHeaders = new StreamHeader[_dataHeader.Streams];
            for (var i = 0; i < _streamHeaders.Length; i++)
            {
                _streamHeaders[i] = new StreamHeader(ref reader);
            }
            //Have all the stream headers now

            _strings = new ClrStrings(GetReaderForStream("#Strings"));
            _userStrings = new ClrUserStrings(GetReaderForStream("#US"));
            _guids = new ClrGuids(GetReaderForStream("#GUID"));
            _tables = new ClrTables(this, GetReaderForStream("#~"));
            _tables.Resolve();
        }

        internal ClrTable<T> GetTable<T>() where T : Row, new() => _tables.GetTable<T>();

        internal IClrTable GetTable(TableFlag flag) => _tables.GetTable(flag);

        internal ClrStrings Strings => _strings;

        internal ClrGuids Guids => _guids;

        private ClrReader GetReaderForStream(string streamName)
        {
            var nameWithNull = streamName + "\0";
            for (var i = 0; i < _streamHeaders.Length; i++)
            {
                if (nameWithNull == _streamHeaders[i].Name)
                {
                    return new ClrReader(_peFile.GetImageDirectory(_header.MetaData).Slice((int)_streamHeaders[i].Offset, (int)_streamHeaders[i].Size));
                }
            }
            throw new InvalidOperationException();
        }
    }
}

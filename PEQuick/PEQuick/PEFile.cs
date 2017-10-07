using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using PEQuick.MetaData;
using PEQuick.PE;
using PEQuick.TableRows;

namespace PEQuick
{
    public class PEFile
    {
        private CliDataHeader _metaDataHeader;
        private MetaDataTables _metaDataTables;
        private StringsSection _strings;
        private UserStringSection _userStrings;
        private GuidSection _guids;
        private BlobSection _blobs;

        internal unsafe PEFile(byte[] file)
        {
            LoadMetaDataHeader();
        }

        internal GuidSection Guids => _guids;
        internal StringsSection Strings => _strings;
        internal BlobSection Blobs => _blobs;
        internal UserStringSection UserStrings => _userStrings;
        internal CliDataHeader CliDataHeader => _metaDataHeader;
        public MetaDataTables MetaDataTables => _metaDataTables;

        private void LoadMetaDataHeader()
        {
            metaData = metaData.ReadMetadataHeader(out _metaDataHeader);

            var streamHeaders = new StreamHeader[_metaDataHeader.Streams];


            for (var i = 0; i < _metaDataHeader.Streams; i++)
            {
                metaData = metaData.ReadStream(out StreamHeader sh);
                streamHeaders[i] = sh;
                switch (sh.Name)
                {
                    case "#Strings":
                        _strings = new StringsSection(section.Slice((int)sh.Offset, (int)sh.Size));
                        break;
                    case "#GUID":
                        _guids = new GuidSection(section.Slice((int)sh.Offset, (int)sh.Size));
                        break;
                    case "#Blob":
                        _blobs = new BlobSection(section.Slice((int)sh.Offset, (int)sh.Size));
                        break;
                    case "#~":
                        break;
                    case "#US":
                        _userStrings = new UserStringSection(section.Slice((int)sh.Offset, (int)sh.Size));
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            //Load the strings and blobs
            var metaTable = streamHeaders.Single(s => s.Name == "#~");
            _metaDataTables = new MetaDataTables(section.Slice((int)metaTable.Offset, (int)metaTable.Size), this);
        }

        internal Section GetImageSection(uint rva)
        {
            foreach (var s in _sections)
            {
                if (rva >= s.VirtualAddress
                    && rva < s.VirtualEnd)
                {
                    return s;
                }
            }
            throw new InvalidOperationException("Could not find the section");
        }

        private Span<byte> GetImageData(ImageDataDirectory dir)
        {
            var address = dir.VirtualAddress + GetImageSection(dir.VirtualAddress).DevirtualisedAddress;
            return _originalFile.AsSpan().Slice((int)address, (int)dir.Size);
        }

        internal Span<byte> GetRVA(uint rva)
        {
            var address = rva + GetImageSection(rva).DevirtualisedAddress;
            return _originalFile.AsSpan().Slice((int)address);
        }
    }
}

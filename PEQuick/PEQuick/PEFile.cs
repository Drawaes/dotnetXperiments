﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick
{
    public class PEFile
    {
        private PEOptions _peOptions;
        private PEHeader _peHeader;
        private uint _peOffset;
        private IMemorySize _memorySize;
        private DataDirectories _dataDirectories;
        private Section[] _sections;
        private CliHeader _cliHeader;
        private byte[] _originalFile;
        private CliDataHeader _metaDataHeader;
        private MetaDataTables _metaDataTables;
        private StringsSection _strings;
        private GuidSection _guidSection;
        private BlobSection _blobs;

        internal unsafe PEFile(byte[] file)
        {
            _originalFile = file;
            var originalSlice = new Span<byte>(file);
            originalSlice.CheckForMagicValue(MagicNumbers.DosMagicNumber);
            //Jump forward another 58 bytes
            originalSlice.Slice(MagicNumbers.PEStartOffset).Read(out _peOffset);
            var s = originalSlice.Slice((int)_peOffset);
            s = s.CheckForMagicValue(MagicNumbers.PEMagicNumber);
            s = s.Read(out _peHeader);
            s = s.Read(out _peOptions);
            s = s.Read(out ushort subsystem);
            s = s.Read(out ushort dllFlags);
            var size = sizeof(MemorySize32);
            if (_peOptions.Is64)
            {
                s = s.Read(out MemorySize64 memSize64);
                _memorySize = memSize64;
            }
            else
            {
                s = s.Read(out MemorySize32 memSize32);
                _memorySize = memSize32;
            }
            s = s.Read(out _dataDirectories);

            _sections = new Section[_peHeader.NumberOfSections];
            for (var i = 0; i < _sections.Length; i++)
            {
                s = s.Read(out _sections[i]);
            }

            LoadCliHeader();
            LoadMetaDataHeader();
        }

        private void LoadMetaDataHeader()
        {
            var metaData = GetImageData(_cliHeader.MetaData);
            var section = metaData;
            metaData = metaData.CheckForMagicValue(MagicNumbers.MetaData);
            metaData = metaData.ReadMetadataHeader(out _metaDataHeader);

            var streamHeaders = new StreamHeader[_metaDataHeader.Streams];


            for (var i = 0; i < _metaDataHeader.Streams; i++)
            {
                metaData = metaData.ReadStream(out StreamHeader sh);
                streamHeaders[i] = sh;
                switch(sh.Name)
                {
                    case "#Strings":
                        _strings = new StringsSection(section.Slice((int)sh.Offset, (int)sh.Size));
                        break;
                    case "#GUID":
                        _guidSection = new GuidSection(section.Slice((int)sh.Offset, (int)sh.Size));
                        break;
                    case "#Blob":
                        _blobs = new BlobSection(section.Slice((int)sh.Offset, (int)sh.Size));
                        break;
                    case "#~":
                        break;
                    case "#US":
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            //Load the strings and blobs
            var metaTable = streamHeaders.Single(s => s.Name == "#~");
            _metaDataTables = new MetaDataTables(section.Slice((int)metaTable.Offset,(int)metaTable.Size), _strings,_blobs);
        }

        private void LoadCliHeader()
        {
            var imageData = GetImageData(_dataDirectories.ClrRuntimeHeader);
            imageData.Read(out _cliHeader);
        }

        private Section GetImageSection(ImageDataDirectory dir)
        {
            foreach (var s in _sections)
            {
                if (dir.VirtualAddress >= s.VirtualAddress
                    && dir.VirtualAddress < s.VirtualEnd)
                {
                    return s;
                }
            }
            throw new InvalidOperationException("Could not find the section");
        }

        private Span<byte> GetImageData(ImageDataDirectory dir)
        {
            var address = dir.VirtualAddress + GetImageSection(dir).DevirtualisedAddress;
            return _originalFile.AsSpan().Slice((int)address, (int)dir.Size);
        }

        public static PEFile Load(string fileName)
        {
            var bytes = System.IO.File.ReadAllBytes(fileName);
            return new PEFile(bytes);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using PEQuick.PE;

namespace PEQuick.Output
{
    public class PEWriter
    {
        private PEFile _inputFile;
        private byte[] _buffer;
        private int _currentIndex;
        private PEOptions _peOptions;
        private int _optionsIndex;
        private DataDirectories _dataDirectories;
        private int _dataDirectoriesIndex;
        private bool _pe64;
        private uint _sizeOfData;
        private uint _entryPointRVA;
        private uint _codeBaseRVA;
        private ulong _imageBaseRVA;
        private uint _sectionAlignment;
        private uint _fileAlignment;

        public PEWriter(PEFile inputFile, bool pe64)
        {
            _fileAlignment = 512;
            _sectionAlignment = 8192;
            _pe64 = pe64;
            _inputFile = inputFile;
        }

        private ushort SizeOfOptionalHeader
        {
            get
            {
                var size = Marshal.SizeOf<PEOptions>() + Marshal.SizeOf<DataDirectories>();
                if (_pe64)
                {
                    size += Marshal.SizeOf<MemorySize64>();
                }
                else
                {
                    size += Marshal.SizeOf<MemorySize32>();
                }
                return (ushort)size;
            }
        }

        private int Index(Span<byte> span) => _buffer.Length - span.Length;

        public void Write(string fileName)
        {
            _buffer = new byte[1024 * 1024 * 100];
            var span = new Span<byte>(_buffer);

            //Write meta data table
            PrepareMetaDataTable();


            span = WriteDOSHeader(span);
            span = WritePEHeader(span);
            span = WritePEOptions(span);
            span = WriteMemory(span);
            span = WriteDataDirectories(span);
        }

        private void PrepareMetaDataTable()
        {
            var remapper = new Dictionary<uint, uint>();
            var blobSection = _inputFile.Blobs.WriteSection(remapper);
            var stringSection = _inputFile.Strings.WriteSection(remapper);
            var userStrings = _inputFile.UserStrings.WriteSection(remapper);
            var metaData = _inputFile.MetaDataTables.Write(remapper, stringSection.Length, blobSection.Length);
            throw new NotImplementedException();
        }

        private Span<byte> WriteDataDirectories(Span<byte> input)
        {
            _dataDirectories = _inputFile.DataDirectories;
            _dataDirectoriesIndex = Index(input);
            input = input.Write(_dataDirectories);
            _currentIndex = Index(input);
            return input;
        }

        private Span<byte> WriteMemory(Span<byte> input)
        {
            if (_pe64)
            {
                input = input.Write(MemorySize32.CreateDefaults());
            }
            else
            {
                input = input.Write(MemorySize64.CreateDefaults());
            }
            _currentIndex = Index(input);
            return input;
        }

        private Span<byte> WriteDOSHeader(Span<byte> input)
        {
            input = input.Write(MagicNumbers.DosHeader);
            _currentIndex = Index(input);
            return input;
        }

        private Span<byte> WritePEHeader(Span<byte> input)
        {
            input = input.Write(MagicNumbers.PEMagicNumber);
            var header = new PEHeader()
            {
                MachineType = _inputFile.PEHeader.MachineType,
                NumberOfSections = 1,
                TimeStamp = _inputFile.PEHeader.TimeStamp,
                OffsetToSymbolTable = 0,
                NumberOfSymbols = 0,
                SizeOfOptionHeader = SizeOfOptionalHeader,
                Characteristics = _inputFile.PEHeader.Characteristics
            };

            input = input.Write(header);
            _currentIndex = Index(input);

            return input;
        }

        private Span<byte> WritePEOptions(Span<byte> input)
        {
            _optionsIndex = Index(input);
            var oldOps = _inputFile.PEOptions;
            _peOptions = new PEOptions()
            {
                Bitness = _pe64 ? Flags.PEFormatType.PE32Plus : Flags.PEFormatType.PE32,
                LMajor = 0x08,
                LMinor = 0x00,
                CodeSize = _sizeOfData,
                InitializedDataSize = 0,
                UninitializedDataSize = 0,
                EntryPointRVA = _entryPointRVA,
                BaseOfCode = _codeBaseRVA,
                ImageBase = _imageBaseRVA,
                SectionAlignment = _sectionAlignment,
                FileAlignment = _fileAlignment,
                OSMajor = 4,
                OSMinor = 0,
                UserMajor = 0,
                UserMinor = 0,
                SubSysMajor = 4,
                SubSysMinor = 0,
                Reserved = 0,
                ImageSize = 0,
                HeaderSize = 0,
                FileChecksum = 0,
                SubSystem = 0x3,
                DllFlags = Flags.DllFlags.IMAGE_DLLCHARACTERISTICS_DYNAMIC_BASE
                | Flags.DllFlags.IMAGE_DLLCHARACTERISTICS_NX_COMPAT
                | Flags.DllFlags.IMAGE_DLLCHARACTERISTICS_NO_SEH
                | Flags.DllFlags.IMAGE_DLLCHARACTERISTICS_TERMINAL_SERVER_AWARE,
            };

            //TODO ImageSize Needs to be the end of the last section rounded up to
            //the section alignment
            //TODO HeaderSize should be the size of all the bytes up to where the data
            //starts
            input = input.Write(_peOptions);
            _currentIndex = Index(input);
            return input;
        }
    }
}

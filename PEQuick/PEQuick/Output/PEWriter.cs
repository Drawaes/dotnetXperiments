using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        private uint _cliCVA;
        private SectionDump _dataSection;

        public PEWriter(PEFile inputFile, bool pe64)
        {
            _fileAlignment = 512;
            _sectionAlignment = 8192;
            _pe64 = pe64;
            _inputFile = inputFile;
            _codeBaseRVA = _sectionAlignment;
            _dataSection = new SectionDump((int)_codeBaseRVA, (int)_sectionAlignment);
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
            _sizeOfData = Utils.Align(_dataSection.VirtualSize, _fileAlignment);

            span = WriteDOSHeader(span);
            var startinex = _currentIndex;
            span = WritePEHeader(span);
            var endindex = _currentIndex - startinex;
            startinex = _currentIndex;
            span = WritePEOptions(span);
            span = WriteMemory(span);
            endindex = _currentIndex - startinex;
            
            span = WriteDataDirectories(span);

            var currentSections = _inputFile.Sections.Where(s => s.Name == 500236121134).Single();
            var section = new Section()
            {
                Characteristics = currentSections.Characteristics,
                Name = currentSections.Name,
                NumberOfLineNumbers = 0,
                NumberOfRelocations = 0,
                PointerToLineNumbers = 0,
                PointerToRelocations = 0,
                PointerToRawData = _fileAlignment,
                VirtualAddress = _dataSection.VirtualAddress,
                SizeOfRawData = _dataSection.RawSize,
                VirtualSize = _dataSection.VirtualSize
            };

            span = span.Write(section);

            var currentIndex = Index(span);
            System.IO.File.WriteAllBytes(@"C:\code\output1.dll", _buffer.AsSpan().Slice(0, currentIndex++).ToArray());

            WriteSection();
            
            _currentIndex += (int) _dataSection.VirtualSize;
            span = new Span<byte>(_buffer,0, _currentIndex++);
            System.IO.File.WriteAllBytes(fileName, span.ToArray());
           
        }

        private void WriteSection()
        {
            _currentIndex = 512;
            var dataSpan = _dataSection.ToSpan();
            var span = new Span<byte>(_buffer, 512);
            dataSpan.CopyTo(span);
        }

        private unsafe void PrepareMetaDataTable()
        {
            var remapper = new Dictionary<uint, uint>();
            var blobSection = _inputFile.Blobs.WriteSection(remapper);
            var stringSection = _inputFile.Strings.WriteSection(remapper);
            var userStrings = _inputFile.UserStrings.WriteSection(remapper);
            var guidSections = _inputFile.Guids.WriteSection(remapper);
            var metaData = _inputFile.MetaDataTables.Write(_dataSection, remapper, stringSection.Length, blobSection.Length);
            
            var cliSize = Marshal.SizeOf<CliHeader>();

            var cliDataHeader = new CliDataHeader()
            {
                Version = _inputFile.CliDataHeader.Version
            };

            var metaHeader = new StreamHeader() { Name = "#~", };
            var guidHeader = new StreamHeader() { Name = "#GUID", };
            var userStringsHeader = new StreamHeader() { Name = "#US", };
            var stringsHeader = new StreamHeader() { Name = "#Strings", };
            var blobsHeader = new StreamHeader() { Name = "#Blob", };

            var cliMetaSize = cliDataHeader.Size;
            var streamHeadersSize = metaHeader.GetSize() + guidHeader.GetSize() + stringsHeader.GetSize() + userStringsHeader.GetSize() + blobsHeader.GetSize();

            var cliOffset = _dataSection.CurrentIndex;
            var metaOffset = cliOffset + cliSize;
            var metaStreamOffset = metaOffset + cliMetaSize + streamHeadersSize;
            var stringStreamOffset = metaStreamOffset + metaData.Length;
            var blobStreamOffset = stringStreamOffset + stringSection.Length;
            var userStreamOffset = blobStreamOffset + blobSection.Length;
            var guidStreamOffset = userStreamOffset + userStrings.Length;

            _cliCVA = (uint)_dataSection.CurrentIndex;
            var index = WriteCliHeader((int)guidStreamOffset - (int)_cliCVA, metaOffset);
            
            Debug.Assert(index == cliOffset);

            _inputFile.CliDataHeader.Streams = 5;
            var headerSpan = _inputFile.CliDataHeader.GetSpan();
            index = _dataSection.WriteData(headerSpan);
            Debug.Assert(index == metaOffset);

            metaHeader.Offset = (uint)metaStreamOffset - (uint)metaOffset;
            metaHeader.Size = (uint)metaData.Length;
            guidHeader.Offset = (uint)guidStreamOffset - (uint)metaOffset; //TODO guid blob,
            guidHeader.Size = (uint)guidSections.Length; //TODO guid Size
            userStringsHeader.Offset = (uint)userStreamOffset - (uint)metaOffset;
            userStringsHeader.Size = (uint)userStrings.Length;
            stringsHeader.Offset = (uint)stringStreamOffset - (uint)metaOffset;
            stringsHeader.Size = (uint)stringSection.Length;
            blobsHeader.Offset = (uint)blobStreamOffset - (uint)metaOffset;
            blobsHeader.Size = (uint)blobSection.Length;

            //write headers
            _dataSection.WriteData(metaHeader.GetSpan());
            _dataSection.WriteData(stringsHeader.GetSpan());
            _dataSection.WriteData(blobsHeader.GetSpan());
            _dataSection.WriteData(userStringsHeader.GetSpan());
            _dataSection.WriteData(guidHeader.GetSpan());
            index = _dataSection.WriteData(metaData);
            //Debug.Assert((index - _cliCVA) == metaHeader.Offset);
            index = _dataSection.WriteData(stringsHeader.GetSpan());
            //Debug.Assert(index == stringsHeader.Offset);
        }

        private unsafe int WriteCliHeader(int cliMetaSize, int metaOffset)
        {
            var oldHeader = _inputFile.CliHeader;
            var cliHeader = new CliHeader()
            {
                CB = (uint)Marshal.SizeOf<CliHeader>(),
                CodeManagerTable = new ImageDataDirectory(),
                EntryPointToken = 0, //TODO find entrypoint
                ExportAddressTableJumps = new ImageDataDirectory(),
                Flags = oldHeader.Flags,
                MajorRuntimeVersion = oldHeader.MajorRuntimeVersion,
                ManagedNativeHeader = new ImageDataDirectory(),
                MetaData = new ImageDataDirectory() { Size = (uint)cliMetaSize, VirtualAddress = (uint)metaOffset },
                MinorRuntimeVersion = oldHeader.MinorRuntimeVersion,
                VTableFixups = new ImageDataDirectory(),
                Resources = new ImageDataDirectory(),
                StrongNameSignatures = new ImageDataDirectory(),
            };

            var s = new Span<byte>(&cliHeader, 72);
            return _dataSection.WriteData(s);
        }

        private Span<byte> WriteDataDirectories(Span<byte> input)
        {
            var oldDirs = _inputFile.DataDirectories;
            
            _dataDirectories = new DataDirectories()
            {
                NumberOfDataDirs = 16,
                LoaderFlags = 0,
                Architecture = new ImageDataDirectory(),
                BoundImport = new ImageDataDirectory(),
                CertificateTable = new ImageDataDirectory(),
                ClrRuntimeHeader = new ImageDataDirectory() { VirtualAddress = _cliCVA, Size = 72 },
                DebugDataDirectory = new ImageDataDirectory(),
                DelayImportDescriptor = new ImageDataDirectory(),
                ExceptionTable = new ImageDataDirectory(),
                ExportTable = new ImageDataDirectory(),
                GlobalPointer = new ImageDataDirectory(),
                BaseRelocationTable = new ImageDataDirectory(), //TODO do we need to write something there?
                IAT = new ImageDataDirectory(), //TODO IAT do we need to write something here?
                ImportTable = new ImageDataDirectory(), //TODO do we need an import table?
                LoadConfigTable = new ImageDataDirectory(),
                Reserved = new ImageDataDirectory(),
                ResourceTable = new ImageDataDirectory(),
                TlsTable = new ImageDataDirectory(),
            };
            _dataDirectoriesIndex = Index(input);
            input = input.Write(_dataDirectories);
            _currentIndex = Index(input);
            return input;
        }

        private Span<byte> WriteMemory(Span<byte> input)
        {
            if (!_pe64)
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
            MagicNumbers.DosHeader.AsSpan().CopyTo(input);
            input = input.Slice(MagicNumbers.DosHeader.Length);
            MagicNumbers.MSDosWarning.AsSpan().CopyTo(input);
            input = input.Slice(MagicNumbers.MSDosWarning.Length);
            _currentIndex = Index(input);
            return input;
        }

        private Span<byte> WritePEHeader(Span<byte> input)
        {
            var oldHeader = _inputFile.PEHeader;
            input = input.Write(MagicNumbers.PEMagicNumber);
            var header = new PEHeader()
            {
                MachineType = _inputFile.PEHeader.MachineType,
                NumberOfSections = 1,
                TimeStamp = _inputFile.PEHeader.TimeStamp,
                OffsetToSymbolTable = 0,
                NumberOfSymbols = 0,
                SizeOfOptionHeader = SizeOfOptionalHeader,
                Characteristics = _inputFile.PEHeader.Characteristics,
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
                InitializedDataSize = _sizeOfData,
                UninitializedDataSize = 0,
                EntryPointRVA = _entryPointRVA,
                BaseOfCode = _dataSection.VirtualAddress,
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

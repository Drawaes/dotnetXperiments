using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

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

        public PEWriter(PEFile inputFile, bool pe64)
        {
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

            span = WriteDOSHeader(span);
            span = WritePEHeader(span);
            span = WritePEOptions(span);
            span = WriteMemory(span);
            span = WriteDataDirectories(span);
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
            if (_inputFile.MemorySizes is MemorySize32 memSize32)
            {
                input.Write(memSize32);
            }
            else if (_inputFile.MemorySizes is MemorySize64 memSize64)
            {
                input.Write(memSize64);
            }
            else
            {
                throw new NotImplementedException();
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
            _peOptions = _inputFile.PEOptions;
            input = input.Write(_peOptions);
            _currentIndex = Index(input);
            return input;
        }
    }
}

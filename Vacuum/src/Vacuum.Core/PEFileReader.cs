using System;
using Vacuum.Core.PE;

namespace Vacuum.Core
{
    public class PEFileReader
    {
        private byte[] _file;
        private int _peHeaderStart;
        private PEHeader _peHeader;
        private PEOptions _peOptions;
        private MemorySizes _memory;
        private DataDirectories _dataDirectories;
        private Section[] _sections;
        private Clr.ClrData _clr;

        public PEFileReader(string filename) => _file = System.IO.File.ReadAllBytes(filename);

        public void Parse()
        {
            var span = _file.AsReadOnlySpan();
            span.CheckForMagicValue(Utils.MagicNumbers.DosMagicNumber);
            span = span.Slice(Utils.MagicNumbers.PEStartOffset);
            span.Read(out _peHeaderStart);

            span = _file.AsReadOnlySpan().Slice(_peHeaderStart);
            span = span.CheckForMagicValue(Utils.MagicNumbers.PEMagicNumber);
            span = span.Read(out _peHeader);
            span = span.Read(out _peOptions);
            _memory = new MemorySizes(ref span, _peOptions.Is64);

            span = span.Read(out _dataDirectories);
            _sections = new Section[_peHeader.NumberOfSections];
            for(var i = 0; i < _sections.Length;i++)
            {
                span = span.Read(out SectionHeader header);
                _sections[i] = new Section(_file, header);
            }

            _clr = new Clr.ClrData(_dataDirectories.ClrRuntimeHeader, this);
        }

        public ReadOnlySpan<byte> GetImageDirectory(ImageDataDirectory directory)
        {
            foreach(var s in _sections)
            {
                if(s.InRange(directory.VirtualAddress) && s.InRange(directory.VirtualAddress + directory.Size))
                {
                    return s.GetSpan(directory);
                }
            }

            throw new ArgumentOutOfRangeException(nameof(directory));
        }
    }
}

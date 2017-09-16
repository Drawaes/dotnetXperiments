using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PEQuick.TableRows;

namespace PEQuick.Importer
{
    public class DependencyWalker
    {
        private PEFile _masterPE;
        private PEFile _sourcePE;

        public DependencyWalker(PEFile masterPE, PEFile sourcePE)
        {
            _masterPE = masterPE;
            _sourcePE = sourcePE;
        }

        public void FindImportPoints()
        {
            var sourceAssembly = _sourcePE.MetaDataTables.GetCollection<AssemblyRow>()[1];
            AssemblyRefRow destAssembly = null;
            foreach (var mod in _masterPE.MetaDataTables.GetCollection<AssemblyRefRow>().Cast<AssemblyRefRow>())
            {
                if (sourceAssembly.Name.Value == mod.Name.Value)
                {
                    destAssembly = mod;
                    break;
                }
                Console.WriteLine($"Assembly referenced {mod.Name.Value}");
            }

            if (destAssembly == null)
            {
                throw new InvalidOperationException();
            }
            var destToken = destAssembly.Tag;



        }

        public void WriteSpan(Span<byte> input, string file)
        {
            var sb = new StringBuilder();
            var counter = 0;
            while (input.Length > 0)
            {
                var sw = input.Slice(0, 8);
                input = input.Slice(8);
                sb.Append(counter.ToString("0000")).Append("    ").AppendLine(BitConverter.ToString(sw.ToArray()));
                counter += 8;
            }
            System.IO.File.WriteAllText(file, sb.ToString());
        }
    }
}

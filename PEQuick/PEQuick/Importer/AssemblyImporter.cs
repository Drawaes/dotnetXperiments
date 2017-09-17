using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Importer
{
    public class AssemblyImporter
    {
        private PEFile _masterPE;
        private PEFile _sourcePE;
        private DependencyGather _dependencies;

        public AssemblyImporter(PEFile masterPE, PEFile sourcePE)
        {
            _masterPE = masterPE;
            _sourcePE = sourcePE;
        }

        private MetaDataTables MasterMeta => _masterPE.MetaDataTables;
        private MetaDataTables SourceMeta => _sourcePE.MetaDataTables;

        public void FindImportPoints()
        {
            var sourceAssembly = SourceMeta.GetCollection<AssemblyRow>()[1];
            AssemblyRefRow destAssembly = null;
            foreach (var mod in MasterMeta.GetCollection<AssemblyRefRow>().Cast<AssemblyRefRow>())
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
            var importingRows = MasterMeta.AssemblyIndexedRows.Where(ai => ai.Key.AssemblyTag == destToken).ToArray();
            var methods = importingRows.Where(ir => ir.Value.Table == TableFlag.MemberRef).Select(m => SourceMeta.FindMethodDef(m.Value));

            _dependencies = new DependencyGather(SourceMeta);
            _dependencies.SeedTags(methods);
            _dependencies.WalkDependencies();
        }
    }
}

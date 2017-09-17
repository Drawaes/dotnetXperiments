using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;
using PEQuick.TableRows;

namespace PEQuick.Importer
{
    public class DependencyGather
    {
        private Dictionary<uint, Row> _touchedRows = new Dictionary<uint, Row>();
        private Queue<uint> _tagsToProcess = new Queue<uint>();
        private MetaDataTables _metaData;

        public DependencyGather(MetaDataTables metadata)
        {
            _metaData = metadata;
        }

        public void SeedTag(uint tag)
        {
            if(_touchedRows.ContainsKey(tag))
            {
                return;
            }
            _touchedRows.Add(tag, _metaData.IndexedRows[tag]);
            _tagsToProcess.Enqueue(tag);
        }

        public void SeedTag(Row row)
        {
            if(row == null)
            {
                return;
            }
            SeedTag(row.Tag);
        }

        public void SeedTags(IEnumerable<uint> tags)
        {
            foreach(var t in tags)
            {
                SeedTag(t);
            }
        }

        public void SeedTags(IEnumerable<Row> tags)
        {
            foreach(var t in tags)
            {
                SeedTag(t.Tag);
            }
        }

        public void WalkDependencies()
        {
            while(_tagsToProcess.Count > 0)
            {
                var tag = _tagsToProcess.Dequeue();
                var row = _metaData.IndexedRows[tag];
                row.GetDependencies(this);
            }
        }
    }
}

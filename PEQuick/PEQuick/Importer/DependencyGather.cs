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
        private Queue<Row> _tagsToProcess = new Queue<Row>();
        private MetaDataTables _metaData;

        public DependencyGather(MetaDataTables metadata)
        {
            _metaData = metadata;
        }

        public Dictionary<uint, Row> Dependencies => _touchedRows;

        public void SeedTag(Row row)
        {
            if (row == null)
            {
                return;
            }
            if (_touchedRows.ContainsKey(row.Tag))
            {
                return;
            }
            _touchedRows.Add(row.Tag, row);
            _tagsToProcess.Enqueue(row);
        }

        public void SeedTags(IEnumerable<Row> tags)
        {
            foreach (var t in tags)
            {
                SeedTag(t);
            }
        }

        public void WalkDependencies()
        {
            while (_tagsToProcess.Count > 0)
            {
                var row = _tagsToProcess.Dequeue();
                row.GetDependencies(this);
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Text;
using PEQuick.MetaData;

namespace PEQuick.TableRows
{
    public abstract class Row
    {
        private int _index;

        public abstract void Read(ref MetaDataReader reader);
        //public abstract void Resolve(MetaDataTables tables);

        public int Index { get => _index; set => _index = value; }
    }
}
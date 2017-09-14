﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PEQuick.Indexes
{
    public struct HasConstantIndex : IIndex
    {
        private uint _rawIndex;

        public void SetRawIndex(uint rawIndex)
        {
            _rawIndex = rawIndex;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace ILSmasher
{
    public class MethodComparer : IComparer<MethodReference>
    {
        public int Compare(MethodReference x, MethodReference y)
        {
            if(x.IsGenericInstance && y.IsGenericInstance) 
            {
                throw new NotImplementedException();
            }
            else if (x.IsGenericInstance || y.IsGenericInstance)
            {
                return -1;
            }
            throw new NotImplementedException();
        }

        public static MethodComparer Instance = new MethodComparer();
    }
}

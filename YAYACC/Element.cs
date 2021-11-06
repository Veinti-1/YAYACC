using System;
using System.Collections.Generic;
using System.Text;

namespace YAYACC
{
    public struct Element
    {
       public string type { get; set; }
        public string value { get; set; }
        public override string ToString()
        {
            return value;
        }
    }
}

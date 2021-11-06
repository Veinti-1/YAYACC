using System;
using System.Collections.Generic;
using System.Text;

namespace YAYACC
{
    class Production
    {
        public List<Element> elements;
        public override string ToString()
        {
            string sOutput = "";
            foreach (var element in elements)
            {
                sOutput += element.ToString() + " ";
            }
            return sOutput;
        }
    }
}

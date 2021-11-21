using System.Collections.Generic;


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

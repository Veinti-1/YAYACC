using System.Collections.Generic;


namespace YAYACC
{
    class Rule
    {
        public string rName = "";
        public List<Production> Productions;

        public override string ToString()
        {
            string sOutput = "\t" + rName + ":\n";
            foreach (var production in Productions)
            {
                sOutput += "\t   " + production.ToString() + "\n";
            }
            return sOutput;
        }
    }
}

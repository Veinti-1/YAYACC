using System;
using System.Collections.Generic;
using System.Text;

namespace YAYACC
{
    class Grammar
    {
        public Rule inicial;
        public List<Rule> Rules;
        public Dictionary<char, char> Alphabet;

        public override string ToString()
        {
            string sOutput = "Inicial Rule: " + inicial.rName + "\n  Rules: \n";
            foreach (var rule in Rules)
            {
                sOutput += rule.ToString();
            }
            sOutput += " Alphabet: \n\t";
            foreach (var item in Alphabet)
            {
                sOutput += item.Value + ", ";
            }
            return sOutput;
        }
    }
}

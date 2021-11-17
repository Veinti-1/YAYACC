using System;
using System.Collections.Generic;
using System.Text;

namespace YAYACC
{
    class Nvals
    {
        public string ruleName;
        public Production myProduction;
        public List<string> lookAhead;
        public int currPos = 0;
        public char action;

        internal string GetNext()
        {
            string outS = ruleName + myProduction.ToString();
            foreach (var item in lookAhead)
            {
                outS += item;
            }
            outS += (currPos+1);
            return outS;
        }
        public override string ToString()
        {
            string outS = ruleName + myProduction.ToString();
            foreach (var item in lookAhead)
            {
                outS += item;
            }
            outS += currPos;
            return outS;
        }
    }
}

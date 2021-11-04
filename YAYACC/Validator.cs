using System;
using System.Collections.Generic;
using System.Text;

namespace YAYACC
{
    class Validator
    {
        private Grammar _gram;
        public Validator(Grammar gram)
        {
            _gram = gram;
        }

        public List<string> validate()
        {
            List<string> EWlist = new List<string>();
            foreach (var rule in _gram.Rules)
            {
                foreach (var production in rule.Productions)
                {
                    foreach (var element in production.elements)
                    {
                        if (element.type == "Nterm")
                        {
                            if (_gram.Rules.Find(x => x.rName == element.value) == null)
                            {
                                EWlist.Add("ERROR: Inside Rule: " + rule.rName + ", " + element.value + " is not specified inside the grammar.");
                            }
                        }
                    }
                }
            }
            string curRuleName = "";
            bool found;
            for (int i = 0; i < _gram.Rules.Count; i++)
            {
                found = false;
                curRuleName = _gram.Rules[i].rName;
                foreach (var rule in _gram.Rules)
                {
                    foreach (var production in rule.Productions)
                    {
                        foreach (var element in production.elements)
                        {
                            if (element.type == "Nterm" && element.value == curRuleName)
                            {
                                found = true;
                                break;
                            }
                        }
                        if (found)
                        {
                            break;
                        }
                    }
                    if (found)
                    {
                        break;
                    }
                }
                if (!found)
                {
                    EWlist.Add("WARNING: Rule: " + curRuleName + " is never referenced inside the grammar.");
                }
            }
   

            return EWlist;
        }
    }
}

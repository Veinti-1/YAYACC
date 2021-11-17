using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YAYACC
{
    class Grammar
    {
        public Rule inicial;
        public List<Rule> Rules;
        public Dictionary<char, char> Alphabet;
        public Dictionary<string, List<string>> Firsts;
        public List<Node> CLRNodes;
        int NodeAmount = 0;// inicia en 0

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
        public void GenerateFirsts()
        {
            Firsts = new Dictionary<string, List<string>>();
            foreach (var rule in Rules)
            {
                GetFirst(rule);
            }
        }

        private void GetFirst(Rule currRule)
        {
            Firsts.TryAdd(currRule.rName, new List<string>());
            foreach (var production in currRule.Productions)
            {
                if (production.elements[0].type == "Term")
                {
                    if (!Firsts[currRule.rName].Any(x => x == production.elements[0].value))
                    {
                        Firsts[currRule.rName].Add(production.elements[0].value);
                    }
                }
                else if (production.elements[0].type == "Nterm")
                {
                    GetFirst(Rules.Find(x => x.rName == production.elements[0].value));
                    Firsts[currRule.rName].AddRange(Firsts[production.elements[0].value]);
                }
            }
        }

        public void GenerateCLR()
        {
            CLRNodes = new List<Node>();
            Element newE = new Element
            {
                type = "Nterm",
                value = inicial.rName
            };
            Production newP = new Production
            {
                elements = new List<Element>
                {
                    newE
                }
            };
            Rules.Add(new Rule
            {
                rName = "aug",
                Productions = new List<Production>
                {
                    newP
                }
            });

           

            GenerateNode(Rules[Rules.Count - 1].rName, newP, 0, new List<string> { "$" });
            //int i = 1;
            //do
            //{
            //    GenerateNode();
            //} while (i < CLRNodes.Count);
        }
        private void GenerateNode(string rName, Production currProd, int pos, List<string> lookAhead)
        {
            Node newNode = new Node
            {
                numNode = NodeAmount,
                nRules = new List<Nvals>(),
                Movements = new Dictionary<Element, int>()
            };

            NodeAmount++;
            Nvals newVals = new Nvals
            {
                ruleName = rName,
                myProduction = currProd,
                lookAhead = lookAhead,
                currPos = pos
            };
            newNode.nRules.Add(newVals);
            CLRNodes.Add(newNode);

            if (newNode.numNode == 23)
            {
                Console.WriteLine("3");
            }

            int i = 0;
            do
            {
                var item = newNode.nRules[i];
                try
                {
                    if (item.myProduction.elements[item.currPos].type == "Nterm")
                    {
                        item.action = 'G';
                        Rule currRule = Rules.Find(x => x.rName == item.myProduction.elements[item.currPos].value);

                        foreach (var production in currRule.Productions)
                        {
                            if (!newNode.nRules.Any(x => x.myProduction == production && x.currPos == 0))
                            {
                                Nvals currVals = new Nvals
                                {
                                    ruleName = currRule.rName,
                                    myProduction = production,
                                    lookAhead = GetLookAhead(item),
                                    currPos = 0
                                };
                                newNode.nRules.Add(currVals);
                            }
                            else
                            {
                                Console.WriteLine("igual");
                            }
                        }

                    }
                    else
                    {
                        item.action = 'S';
                    }
                }
                catch (Exception)
                {
                    item.action = 'R';
                    if (item.ruleName == "aug")
                    {
                        item.action = 'A';
                    }
                }

                i++;
            } while (i < newNode.nRules.Count);


            if (newNode.numNode == 3)
            {
                Console.WriteLine("3");
            }

            bool generate;
            int nextNode;
            foreach (var item in newNode.nRules)
            {
                generate = true;
                nextNode = 0;
                foreach (var node in CLRNodes)
                {
                    if (node.nRules[0].ToString() == item.GetNext() && node.numNode != newNode.numNode)
                    {
                        nextNode = node.numNode;
                        generate = false;
                        break;
                    }
                }
                if (generate)
                {
                    newNode.Movements.TryAdd(item.myProduction.elements[item.currPos], NodeAmount);
                }
                else
                {
                    newNode.Movements.TryAdd(item.myProduction.elements[item.currPos], nextNode);
                }
            }
            Console.WriteLine("fin");
        }


        private List<string> GetLookAhead(Nvals currNval)
        {
            try
            {
                if (currNval.myProduction.elements[currNval.currPos].type == "Nterm")
                {
                    return Firsts[currNval.myProduction.elements[currNval.currPos].value];
                }
                else
                {
                    return new List<string> { currNval.myProduction.elements[currNval.currPos].value };
                }
            }
            catch (Exception)
            {
                return new List<string> { "$" };
            }
        }
    }
}

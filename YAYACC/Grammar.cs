using System;
using System.Collections.Generic;
using System.Linq;

namespace YAYACC
{
    class Grammar
    {
        public Rule inicial;
        public List<Rule> Rules;
        public Dictionary<char, char> Alphabet;
        private Dictionary<string, List<string>> Firsts;
        private List<Node> CLRNodes;
        private List<Node> LALRNodes;
        private int NodeAmount = 0;
        private Stack<string> stack;
        private Stack<int> StateStack;
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
        public bool Parse(string input)
        {
            stack = new Stack<string>();
            StateStack = new Stack<int>();
            StateStack.Push(0);
            return Parse(input + "$", 0);
        }
        private bool Parse(string input, int currState)
        {
            try
            {
                Action currAction = LALRNodes.Find(x => x.numNode == currState).Movements[Convert.ToString(input[0])];
                switch (currAction.pAction)
                {
                    case 'S':
                        stack.Push(Convert.ToString(input[0]));
                        StateStack.Push(currAction.direction);
                        return Parse(input.Substring(1), currAction.direction);
                    case 'G':
                        return Parse(input, currAction.direction);
                    case 'R':
                        if (currAction.rName == "0" && input[0] == '$')
                        {
                            return true;
                        }
                        if (!(currAction.ReduceProd.elements.Count == 1 && currAction.ReduceProd.elements[0].value == "ε"))
                        {
                            int i = currAction.ReduceProd.elements.Count - 1;
                            do
                            {
                                if (stack.Pop() == currAction.ReduceProd.elements[i].value)
                                {
                                    StateStack.Pop();
                                }
                                else
                                {
                                    return false;
                                }
                                i--;
                            } while (i >= 0);
                        }

                        stack.Push(currAction.rName);
                        currAction = LALRNodes.Find(x => x.numNode == StateStack.Peek()).Movements[stack.Peek()];
                        StateStack.Push(currAction.direction);
                        return Parse(input, currAction.direction);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
        private void GenerateFirsts()
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
        private void GenerateCLR()
        {
            GenerateFirsts();
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

            Nvals newNval = new Nvals
            {
                ruleName = "0",
                myProduction = newP,
                lookAhead = new List<string> { "$" },
            };
            GenerateNode(new List<Nvals> { newNval });

            Console.WriteLine("");
            int i = 0;
            do
            {
                CheckNodeGen(i);
                i++;
            } while (i < CLRNodes.Count);
            Console.WriteLine("");
        }
        public void GenerateLALR()
        {
            GenerateCLR();
            LALRNodes = new List<Node>();
            List<int> removedDups = new List<int>();
            foreach (var node in CLRNodes)
            {
                for (int i = 0; i < CLRNodes.Count; i++)
                {
                    string a = node.GetKernelStr();
                    string b = CLRNodes[i].GetKernelStr();
                    if (a == b && node.numNode != i)
                    {
                        if (!removedDups.Any(x => x == node.numNode))
                        {
                            removedDups.Add(node.numNode);
                            removedDups.Add(i);
                            string newNum = "";
                            newNum += node.numNode;
                            newNum += i;
                            Node fusedNode = new Node
                            {
                                Kernels = node.Kernels,
                                numNode = Convert.ToInt32(newNum),
                                nRules = node.nRules,
                                Movements = node.Movements
                            };
                            LALRNodes.Add(fusedNode);
                            int j = 0;
                            foreach (var nNval in LALRNodes[LALRNodes.Count - 1].nRules)
                            {
                                nNval.lookAhead.AddRange(CLRNodes[i].nRules[j].lookAhead);
                                nNval.lookAhead = nNval.lookAhead.Distinct().ToList();
                                if (nNval.action == 'R')
                                {
                                    foreach (var item in nNval.lookAhead)
                                    {
                                        LALRNodes[LALRNodes.Count - 1].Movements.TryAdd(item, new Action { pAction = 'R', rName = nNval.ruleName, ReduceProd = nNval.myProduction });
                                    }
                                }
                                j++;
                            }
                        }
                    }
                }
            }
            foreach (var node in LALRNodes)
            {
                foreach (var item in node.Movements)
                {
                    int transition = item.Value.direction;
                    int index = removedDups.IndexOf(transition);
                    if (index >= 0)
                    {
                        string numConcat = "";
                        if (index % 2 == 0)
                        {
                            numConcat += removedDups[index];
                            numConcat += removedDups[index + 1];
                        }
                        else
                        {
                            numConcat += removedDups[index - 1];
                            numConcat += removedDups[index];
                        }
                        item.Value.direction = Convert.ToInt32(numConcat);
                    }
                }
            }
            for (int i = 0; i < CLRNodes.Count; i++)
            {
                if (!removedDups.Any(x => x == i))
                {
                    foreach (var item in CLRNodes[i].Movements)
                    {
                        int transition = item.Value.direction;
                        int index = removedDups.IndexOf(transition);
                        if (index >= 0)
                        {
                            string numConcat = "";
                            if (index % 2 == 0)
                            {
                                numConcat += removedDups[index];
                                numConcat += removedDups[index + 1];
                            }
                            else
                            {
                                numConcat += removedDups[index - 1];
                                numConcat += removedDups[index];
                            }
                            item.Value.direction = Convert.ToInt32(numConcat);
                        }
                    }
                    LALRNodes.Add(CLRNodes[i]);
                }
            }
        }
        private void GenerateNode(List<Nvals> Kernels)
        {
            Node newNode = new Node
            {
                Kernels = Kernels,
                numNode = NodeAmount,
                nRules = new List<Nvals>(),
                Movements = new Dictionary<string, Action>()
            };

            NodeAmount++;

            newNode.nRules.AddRange(Kernels);
            CLRNodes.Add(newNode);

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
        }
        private Dictionary<string, List<Nvals>> GenerateKernels(int nodeNum)
        {
            Dictionary<string, List<Nvals>> newKernels = new Dictionary<string, List<Nvals>>();
            foreach (var NodeRule in CLRNodes[nodeNum].nRules)
            {
                char currAction = 'R';
                Nvals newGenNval = new Nvals
                {
                    ruleName = NodeRule.ruleName,
                    myProduction = NodeRule.myProduction,
                    lookAhead = NodeRule.lookAhead,
                    currPos = NodeRule.currPos + 1,
                };
                try
                {
                    string currVal = NodeRule.myProduction.elements[NodeRule.currPos].value;
                    switch (NodeRule.myProduction.elements[NodeRule.currPos].type)
                    {
                        case "Nterm":
                            currAction = 'G';
                            break;
                        case "Term":
                            currAction = 'S';
                            break;
                    }
                    if (currVal != "ε" && newKernels.TryAdd(currVal, new List<Nvals> { newGenNval }))
                    {
                        CLRNodes[nodeNum].Movements.Add(currVal, new Action { pAction = currAction });
                    }
                    else
                    {
                        newKernels[currVal].Add(newGenNval);
                    }
                }
                catch (Exception)
                {
                    try
                    {
                        foreach (var item in NodeRule.lookAhead)
                        {
                            CLRNodes[nodeNum].Movements.Add(item, new Action { pAction = 'R', rName = NodeRule.ruleName, ReduceProd = NodeRule.myProduction });
                        }
                    }
                    catch (Exception)
                    {
                        throw new Exception("Shift Reduce Conflict");
                    }
                }
            }
            return newKernels;
        }
        private void CheckNodeGen(int nodeNum)
        {
            bool generar = true;
            foreach (var kernel in GenerateKernels(nodeNum))
            {
                generar = true;
                string sKernel = "";
                string sKernelComp = "";
                foreach (var nval in kernel.Value)
                {
                    if (nval.myProduction.elements.Any(x => x.value == "ε"))
                    {
                        generar = false;
                        foreach (var item in nval.lookAhead)
                        {
                            CLRNodes[nodeNum].Movements.Add(item, new Action { pAction = 'R', rName = nval.ruleName, ReduceProd = nval.myProduction });
                        }
                        break;
                    }
                    sKernel += nval.ToString();
                }
                if (generar)
                {
                    foreach (var node in CLRNodes)
                    {
                        sKernelComp = "";
                        foreach (var Nodenval in node.Kernels)
                        {
                            sKernelComp += Nodenval.ToString();
                        }
                        if (sKernel == sKernelComp)
                        {
                            CLRNodes[nodeNum].Movements[kernel.Key].direction = node.numNode;
                            generar = false;
                            break;
                        }
                    }
                }
                if (generar)
                {
                    CLRNodes[nodeNum].Movements[kernel.Key].direction = NodeAmount;
                    GenerateNode(kernel.Value);
                }
            }
        }
        private List<string> GetLookAhead(Nvals currNval)
        {
            try
            {
                if (currNval.myProduction.elements[currNval.currPos + 1].type == "Nterm")
                {
                    List<string> newLookAheads = new List<string>();

                    int i = 1;
                    do
                    {
                        try
                        {
                            if (currNval.myProduction.elements[currNval.currPos + i].type == "Nterm")
                            {
                                if (Firsts[currNval.myProduction.elements[currNval.currPos + i].value].Any(x => x == "ε"))
                                {
                                    newLookAheads.AddRange(currNval.lookAhead);
                                }
                            }
                            else
                            {
                                newLookAheads.Add(currNval.myProduction.elements[currNval.currPos + 2].value);
                                break;
                            }
                        }
                        catch (Exception)
                        {
                            break;
                        }
                        i++;
                    } while (true);

                    newLookAheads.AddRange(Firsts[currNval.myProduction.elements[currNval.currPos + 1].value]);
                    newLookAheads = newLookAheads.Distinct().ToList();
                    newLookAheads.Remove("ε");
                    return newLookAheads;
                }
                else
                {
                    return new List<string> { currNval.myProduction.elements[currNval.currPos + 1].value };
                }
            }
            catch (Exception)
            {
                return currNval.lookAhead;
            }
        }
    }
}

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
        private Dictionary<string, List<string>> Firsts;
        private List<Node> CLRNodes;
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
                Action currAction = CLRNodes[currState].Movements[Convert.ToString(input[0])];
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
                        int i = currAction.ReduceProd.elements.Count - 1;
                        do
                        {
                            if (stack.Pop() == currAction.ReduceProd.elements[i].value)
                            {
                                StateStack.Pop();
                            }
                            else
                            {
                                //throw new Exception("Invalid input for the grammar");
                                return false;
                            }
                            i--;
                        } while (i >= 0);
                        stack.Push(currAction.rName);
                        currAction = CLRNodes[StateStack.Peek()].Movements[stack.Peek()];
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
        public void GenerateCLR()
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
                    if (newKernels.TryAdd(currVal, new List<Nvals> { newGenNval }))
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
            if (nodeNum ==1)
            {
                Console.WriteLine();
            }
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
                            CLRNodes[nodeNum].Movements.Add(item, new Action { pAction = 'R' });
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

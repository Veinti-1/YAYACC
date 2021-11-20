using System;
using System.Collections.Generic;
using System.Text;

namespace YAYACC
{
    class Node
    {
        public List<Nvals> Kernels;
        public int numNode;
        public List<Nvals> nRules;
        public Dictionary<string, Action> Movements;
    }
}

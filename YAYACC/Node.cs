using System.Collections.Generic;


namespace YAYACC
{
    class Node
    {
        public List<Nvals> Kernels;
        public int numNode;
        public List<Nvals> nRules;
        public Dictionary<string, Action> Movements;
        public string GetKernelStr()
        {
            string output = "";
            foreach (var kernel in Kernels)
            {
                output += kernel.ruleName + kernel.myProduction.ToString() + kernel.currPos;
            }
            return output;
        }
    }
}

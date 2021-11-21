

namespace YAYACC
{
    public struct Element
    {
       public string type { get; set; }
        public string value { get; set; }
        public override string ToString()
        {
            return value;
        }
    }
}

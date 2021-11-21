

namespace YAYACC
{
    public enum TokenType
    {
        /*
            Colon -> :
            Semicolon -> ;
            Pipe -> |
            NTerm -> [a-zA-Z_][a-zA-Z0-9_]*
            Term -> '[a-ZA-Z0-9!"#%&()\ast+,-./:;<=>?[]^_{|}~] | \[nt\']' 
         */
        Colon = ':',
        Semicolon = ';',
        Pipe = '|',
        EOF = (char)0,
        NTerm = (char)3,
        Term = (char)4
    }
}

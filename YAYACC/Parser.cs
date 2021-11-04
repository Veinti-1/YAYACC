using System;
using System.Collections.Generic;
using System.Text;

namespace YAYACC
{
    class Parser
    {
        Scanner scanner;
        Token nextToken;
        Grammar newGrammar;
        private void S()
        {
            switch (nextToken.Tag)
            {
                case TokenType.NTerm:
                    Match(TokenType.NTerm);
                    Match(TokenType.Colon);
                    A();
                    B();
                    Match(TokenType.Semicolon);
                    C();
                    break;
                default:
                    //error
                    break;
            }
        }
        private void A()
        {
            switch (nextToken.Tag)
            {
                case TokenType.NTerm:
                    Match(TokenType.NTerm); // IR LLENANDO LA LISTA DE PASOS
                    X();
                    break;
                case TokenType.Term:
                    Match(TokenType.Term);
                    X();
                    break;
                default:
                    //error
                    break;
            }
        }
        private void X()
        {
            switch (nextToken.Tag)
            {
                case TokenType.NTerm:
                case TokenType.Term:
                    A();
                    break;
                default:
                    //retornar nulo
                    break;
            }
        }

        private void B()
        {
            switch (nextToken.Tag)
            {
                case TokenType.Pipe:
                    Match(TokenType.Pipe);
                    A(); //agregar salida de esta a la lista de opciones de esa regla
                    B();
                    break;
                default:
                    //retornar nulo
                    break;
            }
        }
        private void C()
        {
            switch (nextToken.Tag)
            {
                case TokenType.NTerm:
                    S();
                    break;
                default:
                    //retornar nulo
                    break;
            }
        }
        
        private void Match(TokenType tag)
        {
            if (nextToken.Tag == tag)
            {
                nextToken = scanner.GetToken();
            }
            else
            {
                throw new Exception("Error de sintaxis, Caracter esperado: " + tag);
            }
        }

        public Grammar Parse(string regexp)
        {
            newGrammar = new Grammar();
            scanner = new Scanner(regexp + (char)TokenType.EOF);
            nextToken = scanner.GetToken();
            switch (nextToken.Tag)
            {
                case TokenType.NTerm:
                    S();
                    break;
                default:
                    //error
                    break;
            }
            Match(TokenType.EOF);
        }
    }
}

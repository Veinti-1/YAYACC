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
        private Rule S()
        {
            switch (nextToken.Tag)
            {
                case TokenType.NTerm:
                    Rule newRule = new Rule();
                    newRule.Productions = new List<Production>();
                    newRule.rName = nextToken.Value;
                    Match(TokenType.NTerm);
                    Match(TokenType.Colon);
                    newRule.Productions.Add(A());

                    try
                    {
                        newRule.Productions.AddRange(B());
                    }
                    catch (Exception)
                    {
                    }
                    
                    Match(TokenType.Semicolon);
                    newGrammar.Rules.Add(newRule);

                    //try
                    //{
                    //    newGrammar.Rules.Add(C());
                    //}
                    //catch (Exception)
                    //{
                    //}
                    C();
                    
                    return newRule;
                default:
                    throw new Exception("Error de sintaxis");
            }
        }
        private Production A()
        {
            Production newProduction = new Production();
            newProduction.elements = new List<Element>();
            switch (nextToken.Tag)
            {
                case TokenType.NTerm:
                    newProduction.elements.Add(Match(TokenType.NTerm)); // IR LLENANDO LA LISTA DE PASOS
                    try
                    {
                        newProduction.elements.Add(X().elements[0]);
                    }
                    catch (Exception)
                    {
                    }
                 
                    
                    return newProduction;
                case TokenType.Term:
                    newProduction.elements.Add(Match(TokenType.Term)); // IR LLENANDO LA LISTA DE PASOS
                    try
                    {
                        newProduction.elements.Add(X().elements[0]);
                    }
                    catch (Exception)
                    {
                    }
                    return newProduction;
                default:
                    throw new Exception("Error de sintaxis");
            }
        }
        private Production X()
        {
            switch (nextToken.Tag)
            {
                case TokenType.NTerm:
                case TokenType.Term:
                    return A();
                default:
                    return null;
            }
        }

        private List<Production> B()
        {
            List<Production> newProductions = new List<Production>();
            switch (nextToken.Tag)
            {
                case TokenType.Pipe:
                    Match(TokenType.Pipe);
                    newProductions.Add(A()); //agregar salida de esta a la lista de opciones de esa regla

                    try
                    {
                        newProductions.Add(B()[0]);
                    }
                    catch (Exception)
                    {
                    }
                   
                    return newProductions;
                default:
                    return null;
            }
        }
        private Rule C()
        {
            switch (nextToken.Tag)
            {
                case TokenType.NTerm:
                    return S();
                default:
                    return null;
            }
        }

        private Element Match(TokenType tag)
        {
            if (nextToken.Tag == tag)
            {
                Element newElement = new Element();
                switch (tag)
                {
                    case TokenType.NTerm:
                        newElement.type = "Nterm";
                        newElement.value = nextToken.Value;
                        break;
                    case TokenType.Term:
                        newElement.type = "Term";
                        newElement.value = nextToken.Value;
                        break;
                    default:
                        break;
                }
                nextToken = scanner.GetToken();
                return newElement;
            }
            else
            {
                throw new Exception("Error de sintaxis, Caracter esperado: " + tag);
            }
        }

        public Grammar Parse(string regexp)
        {
            newGrammar = new Grammar();
            newGrammar.Rules = new List<Rule>();
            scanner = new Scanner(regexp + (char)TokenType.EOF);
            nextToken = scanner.GetToken();
            switch (nextToken.Tag)
            {
                case TokenType.NTerm:
                    newGrammar.inicial = S();
                    break;
                default:
                    //error
                    break;
            }
            Match(TokenType.EOF);
            return newGrammar;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace YAYACC
{
    class Parser
    {
        //Scanner scanner;
        //Token nextToken;

        //private double E()
        //{
        //    switch (nextToken.Tag)
        //    {
        //        case TokenType.Minus:
        //        case TokenType.Num:
        //        case TokenType.LParen:
        //            return T() + EP();
        //        default:
        //            throw new Exception("Error de sintaxis");
        //    }
        //}
        //private double EP()
        //{
        //    switch (nextToken.Tag)
        //    {
        //        case TokenType.Plus:
        //            Match(TokenType.Plus);
        //            return T() + EP();
        //        case TokenType.Minus:
        //            Match(TokenType.Minus);
        //            return -T() + EP();
        //        default:
        //            return 0.0;
        //    }
        //}
        //private double T()
        //{
        //    switch (nextToken.Tag)
        //    {
        //        case TokenType.Minus:
        //        case TokenType.Num:
        //        case TokenType.LParen:
        //            return F() * TP();
        //        default:
        //            throw new Exception("Error de sintaxis");
        //    }
        //}
        //private double TP()
        //{
        //    switch (nextToken.Tag)
        //    {
        //        case TokenType.Mult:
        //            Match(TokenType.Mult);
        //            return F() * TP();
        //        case TokenType.Div:
        //            Match(TokenType.Div);
        //            return (1 / F()) * TP();
        //        default:
        //            return 1.0;
        //    }
        //}
        //private double F()
        //{
        //    switch (nextToken.Tag)
        //    {
        //        case TokenType.Minus:
        //            Match(TokenType.Minus);
        //            return -M();
        //        case TokenType.Num:
        //        case TokenType.LParen:
        //            return M();
        //        default:
        //            throw new Exception("Error de sintaxis");
        //    }
        //}
        //private double M()
        //{
        //    switch (nextToken.Tag)
        //    {
        //        case TokenType.Num:
        //            return Match(TokenType.Num);
        //        case TokenType.LParen:
        //            Match(TokenType.LParen);
        //            double outE = E();
        //            Match(TokenType.RParen);
        //            return outE;
        //        default:
        //            return 0.0;//throw new Exception("Error de sintaxis");
        //    }
        //}
        //private double Match(TokenType tag)
        //{
        //    double output = 0;
        //    if (nextToken.Tag == tag)
        //    {
        //        if (nextToken.Tag == TokenType.Num)
        //        {
        //            output = Convert.ToDouble(nextToken.Value);
        //        }
        //        nextToken = scanner.GetToken();
        //    }
        //    else
        //    {
        //        throw new Exception("Error de sintaxis, Caracter esperado: " + tag);
        //    }
        //    return output;
        //}

        //public double Parse(string regexp)
        //{
        //    double output = 0;
        //    scanner = new Scanner(regexp + (char)TokenType.EOF);
        //    nextToken = scanner.GetToken();
        //    switch (nextToken.Tag)
        //    {
        //        case TokenType.Minus:
        //        case TokenType.Num:
        //        case TokenType.LParen:
        //            output = E();
        //            break;
        //        default:
        //            break;
        //    }
        //    Match(TokenType.EOF);
        //    return output;
        //}
    }
}

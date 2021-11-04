using System;
using System.Collections.Generic;
using System.Text;

namespace YAYACC
{
    class Scanner
    {
        private string _regexp = "";
        private int _index;
        private int _currState;
        public Scanner(string regexp)
        {
            _regexp = regexp + (char)TokenType.EOF;
            _index = 0;
            _currState = 0;
        }
        public Token GetToken()
        {
            Token Tresult = new Token()
            {
                Value = ""
            };
            bool tokenFound = false;
            while (!tokenFound)
            {
                char peek = _regexp[_index];
                switch (_currState)
                {
                    case 0:
                        while (char.IsWhiteSpace(peek))
                        {
                            _index++;
                            peek = _regexp[_index];
                        }
                        switch (peek)
                        {
                            case (char)TokenType.Colon:
                            case (char)TokenType.Semicolon:
                            case (char)TokenType.Pipe:
                            case (char)TokenType.EOF:
                                tokenFound = true;
                                Tresult.Tag = (TokenType)peek;
                                break;
                            case '\'':
                                _currState = 2;
                                break;

                            case '_':
                                Tresult.Value += peek;
                                _currState = 1;
                                break;
                            default:
                                if ((peek > 64 && peek < 91) || (peek > 96 && peek < 123))
                                {
                                    Tresult.Value += peek;
                                    _currState = 1;
                                }
                                else
                                {
                                    throw new Exception("Lex Error");
                                }
                                break;
                        }
                        break;
                    case 1:
                        switch (peek)
                        {
                            case '0':
                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                            case '_':
                                Tresult.Value += peek;
                                _currState = 1;
                                break;
                            case ' ':
                            case '\n':
                            case (char)TokenType.EOF:
                                _index--;
                                tokenFound = true;
                                Tresult.Tag = TokenType.NTerm;
                                break;
                            default:
                                if ((peek > 64 && peek < 91) || (peek > 96 && peek < 123))
                                {
                                    Tresult.Value += peek;
                                    _currState = 1;
                                }
                                else
                                {
                                    throw new Exception("Lex Error");
                                }
                                break;
                        }
                        break;
                    case 2:
                        if ((peek > 64 && peek < 91) || (peek > 96 && peek < 123))
                        {
                            Tresult.Value += peek;
                            _currState = 3;
                        }
                        else
                        {
                            switch (peek)
                            {
                                case '!':
                                case '"':
                                case '#':
                                case '%':
                                case '&':
                                case '(':
                                case ')':
                                case '*':
                                case '+':
                                case ',':
                                case '-':
                                case '.':
                                case '/':
                                case ':':
                                case ';':
                                case '<':
                                case '=':
                                case '>':
                                case '?':
                                case '[':
                                case ']':
                                case '^':
                                case '_':
                                case '{':
                                case '|':
                                case '}':
                                case '~':
                                case ' ':
                                    Tresult.Value += peek;
                                    _currState = 3;
                                    break;
                                case '\\':
                                    _currState = 5;
                                    break;
                                default:
                                    throw new Exception("Lex Error");
                            }
                        }
                        break;
                    case 3:
                        switch (peek)
                        {
                            case '\'':
                                tokenFound = true;
                                Tresult.Tag = TokenType.Term;
                                break;                            
                            default:
                                throw new Exception("Lex Error");
                        }
                        break;
                    case 4:
                        break;
                    case 5:
                        switch (peek)
                        {
                            case 'n':
                                Tresult.Value += '\n';
                                _currState = 3;
                                break;
                            case 't':
                                Tresult.Value += '\t';
                                _currState = 3;
                                break;
                            case '\\':
                                Tresult.Value += '\\';
                                _currState = 3;
                                break;
                            case '\'':
                                Tresult.Value += '\'';
                                _currState = 3;
                                break;
                            default:
                                throw new Exception("Lex Error");
                        }
                        break;
                }
                _index++;
            }

            _currState = 0;
            return Tresult;
        }
    }
}

using System;
using System.Collections.Generic;

namespace LearnInterpreter
{
    public class Lexer
    {
        private string text;
        private char? currentChar;

        private int readPosition;

        private Dictionary<string, Token> reservedKeywords = new Dictionary<string, Token>()
        {
            { "decimal", new Token(TokenType.Type, "decimal") },
            { "void", new Token(TokenType.Void, "void") }
        };

        public Lexer(string text)
        {
            this.text = text;
            currentChar = text[readPosition];
        }

        public Token NextToken()
        {
            while (currentChar != null) //if isn't EOF
            {
                if (char.IsWhiteSpace((char)currentChar) || currentChar == '\r' || currentChar == '\n')
                {
                    SkipWhitespace();
                    continue;
                }

                if (currentChar == '/' && Peek() == '/') // comment handling
                {
                    SkipComment();
                    continue;
                }

                if (char.IsLetter((char)currentChar))
                {
                    return Identifier();
                }

                if (currentChar == '{')
                {
                    Advance();
                    return new Token(TokenType.OpenBracket, "{");
                }

                if (currentChar == '}')
                {
                    Advance();
                    return new Token(TokenType.CloseBracket, "}");
                }

                if (currentChar == '=')
                {
                    Advance();
                    return new Token(TokenType.Assign, "=");
                }

                if (currentChar == ';')
                {
                    Advance();
                    return new Token(TokenType.Semicolon, ";");
                }

                if (char.IsDigit((char)currentChar))
                {
                    return new Token(TokenType.Integer, Integer());
                }

                if (currentChar == '+')
                {
                    Advance();
                    return new Token(TokenType.Plus, "+");
                }

                if (currentChar == '-')
                {
                    Advance();
                    return new Token(TokenType.Minus, "-");
                }

                if (currentChar == '*')
                {
                    Advance();
                    return new Token(TokenType.Mult, "*");
                }

                if (currentChar == '/')
                {
                    Advance();
                    return new Token(TokenType.Div, "/");
                }

                if (currentChar == '(')
                {
                    Advance();
                    return new Token(TokenType.LeftParen, "(");
                }

                if (currentChar == ')')
                {
                    Advance();
                    return new Token(TokenType.RightParen, ")");
                }

                if (currentChar == '.')
                {
                    Advance();
                    return new Token(TokenType.Dot, ".");
                }

                throw new Exception("Invalid syntax!");
            }

            return new Token(TokenType.Eof, string.Empty);
        }

        private Token Identifier()
        {
            string result = string.Empty;

            while (currentChar != null && char.IsLetterOrDigit((char)currentChar))
            {
                result += currentChar;
                Advance();
            }

            if (reservedKeywords.TryGetValue(result, out Token token))
            {
                return token;
            }

            return new Token(TokenType.Identifier, result);
        }

        private char? Peek()
        {
            int peekPos = readPosition + 1;
            if (peekPos >= text.Length)
            {
                return null;
            }
            else
            {
                return text[peekPos];
            }
        }

        private void Advance()
        {
            readPosition++;
            if (readPosition >= text.Length)
                currentChar = null;
            else
                currentChar = text[readPosition];
        }

        private void SkipWhitespace()
        {
            while (currentChar != null && (char.IsWhiteSpace((char)currentChar) || currentChar == '\r' || currentChar == '\n'))
            {
                Advance();
            }
        }

        private void SkipComment()
        {
            while (currentChar != '\r' && currentChar != '\n')
            {
                Advance();
            }
        }

        private string Integer()
        {
            string str = string.Empty;
            while (currentChar != null && char.IsDigit((char)currentChar))
            {
                str += currentChar;
                Advance();
            }

            return str;
        }
    }
}

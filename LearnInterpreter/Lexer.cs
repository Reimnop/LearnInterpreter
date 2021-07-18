using System;
using System.Collections.Generic;

namespace LearnInterpreter
{
    public class Lexer
    {
        private string text;
        private char? currentChar;

        private int readPosition;
        private int line = 1;
        private int column = 1;

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

                if (char.IsDigit((char)currentChar))
                {
                    return new Token(TokenType.Integer, Integer(), line, column);
                }

                if (char.IsLetter((char)currentChar))
                {
                    return Identifier();
                }

                if (currentChar == '/' && Peek() == '/') // comment handling
                {
                    SkipComment();
                    continue;
                }

                //deal with single char tokens
                if (Enum.IsDefined(typeof(TokenType), (int)currentChar))
                {
                    TokenType tokenType = (TokenType)currentChar;
                    Token token = new Token(tokenType, currentChar.ToString(), line, column);
                    Advance();
                    return token;
                }

                ThrowError();
            }

            return new Token(TokenType.Eof, string.Empty);
        }

        private void ThrowError()
        {
            throw new LexerError(string.Empty, null, $"On {currentChar}, line {line}, column {column}");
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

            return new Token(TokenType.Identifier, result, line, column);
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
            if (currentChar == '\n')
            {
                line++;
                column = 0;
            }

            readPosition++;
            if (readPosition >= text.Length)
            {
                currentChar = null;
            }
            else
            {
                currentChar = text[readPosition];
                column++;
            }
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

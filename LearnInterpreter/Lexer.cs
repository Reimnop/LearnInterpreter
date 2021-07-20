using System;
using System.Collections.Generic;

namespace LearnInterpreter
{
    public class Lexer
    {
        public char? CurrentChar => _currentChar;

        private string text;
        private char? _currentChar;

        private int readPosition;
        private int line = 1;
        private int column = 1;

        private Dictionary<string, Token> reservedKeywords = new Dictionary<string, Token>()
        {
            { "var", new Token(TokenType.Var, "var") },
            { "function", new Token(TokenType.Function, "function") },
            { "if", new Token(TokenType.If, "if") },
            { "true", new Token(TokenType.True, "true") },
            { "false", new Token(TokenType.False, "false") },
            { "return", new Token(TokenType.Return, "return") },
            { "for", new Token(TokenType.For, "for") },
            { "while", new Token(TokenType.While, "while") }
        };

        public Lexer(string text)
        {
            this.text = text;
            _currentChar = text[readPosition];
        }

        public Token NextToken()
        {
            while (_currentChar != null) //if isn't EOF
            {
                if (char.IsWhiteSpace((char)_currentChar) || _currentChar == '\r' || _currentChar == '\n')
                {
                    SkipWhitespace();
                    continue;
                }

                if (_currentChar == '/' && Peek() == '/') // comment handling
                {
                    SkipComment();
                    continue;
                }

                if (char.IsDigit((char)_currentChar))
                {
                    return new Token(TokenType.Integer, Integer(), line, column);
                }

                if (char.IsLetter((char)_currentChar))
                {
                    return Identifier();
                }

                if (_currentChar == '"')
                {
                    return String();
                }

                if (_currentChar == '=' && Peek() == '=')
                {
                    Advance();
                    Advance();
                    return new Token(TokenType.Equal, "==", line, column);
                }

                if (_currentChar == '!' && Peek() == '=')
                {
                    Advance();
                    Advance();
                    return new Token(TokenType.NotEqual, "!=", line, column);
                }

                if (_currentChar == '<' && Peek() == '=')
                {
                    Advance();
                    Advance();
                    return new Token(TokenType.LessThanOrEqualTo, "<=", line, column);
                }

                if (_currentChar == '>' && Peek() == '=')
                {
                    Advance();
                    Advance();
                    return new Token(TokenType.GreaterThanOrEqualTo, ">=", line, column);
                }

                //deal with single char tokens
                if (Enum.IsDefined(typeof(TokenType), (int)_currentChar))
                {
                    TokenType tokenType = (TokenType)_currentChar;
                    Token token = new Token(tokenType, _currentChar.ToString(), line, column);
                    Advance();
                    return token;
                }

                ThrowError();
            }

            return new Token(TokenType.Eof, string.Empty);
        }

        private void ThrowError()
        {
            throw new LexerError(string.Empty, null, $"On {_currentChar}, line {line}, column {column}");
        }

        private Token Identifier()
        {
            string result = string.Empty;

            while (_currentChar != null && char.IsLetterOrDigit((char)_currentChar))
            {
                result += _currentChar;
                Advance();
            }

            if (reservedKeywords.TryGetValue(result, out Token token))
            {
                return token.CopyNewPos(line, column);
            }

            return new Token(TokenType.Identifier, result, line, column);
        }

        private Token String()
        {
            string result = string.Empty;

            Advance();
            while (_currentChar != null && _currentChar != '"')
            {
                result += _currentChar;
                Advance();
            }
            Advance();

            return new Token(TokenType.String, result, line, column);
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
            if (_currentChar == '\n')
            {
                line++;
                column = 0;
            }

            readPosition++;
            if (readPosition >= text.Length)
            {
                _currentChar = null;
            }
            else
            {
                _currentChar = text[readPosition];
                column++;
            }
        }

        private void SkipWhitespace()
        {
            while (_currentChar != null && (char.IsWhiteSpace((char)_currentChar) || _currentChar == '\r' || _currentChar == '\n'))
            {
                Advance();
            }
        }

        private void SkipComment()
        {
            while (_currentChar != '\r' && _currentChar != '\n')
            {
                Advance();
            }
        }

        private string Integer()
        {
            string str = string.Empty;
            while (_currentChar != null && char.IsDigit((char)_currentChar))
            {
                str += _currentChar;
                Advance();
            }

            return str;
        }
    }
}

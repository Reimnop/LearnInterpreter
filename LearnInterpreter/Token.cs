namespace LearnInterpreter
{
    public class Token
    {
        public TokenType TokenType => _tokenType;
        public string Value => _value;
        public int Line => _line;
        public int Column => _column;

        private TokenType _tokenType;
        private string _value;
        private int _line;
        private int _column;

        public Token(TokenType tokenType, string value, int line = 0, int column = 0)
        {
            _tokenType = tokenType;
            _value = value;
            _line = line;
            _column = column;
        }

        public int AsInteger()
        {
            return int.Parse(_value);
        }

        public float AsFloat()
        {
            return float.Parse(_value);
        }

        public override string ToString()
        {
            return $"Token({_tokenType}, {_value}, position = {_line}:{_column})";
        }
    }
}

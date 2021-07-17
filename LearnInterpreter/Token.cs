namespace LearnInterpreter
{
    public class Token
    {
        public TokenType TokenType => _tokenType;
        public string Value => _value;

        private TokenType _tokenType;
        private string _value;

        public Token(TokenType tokenType, string value)
        {
            _tokenType = tokenType;
            _value = value;
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
            return $"Token({_tokenType}, {_value})";
        }
    }
}

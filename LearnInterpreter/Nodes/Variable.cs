namespace LearnInterpreter
{
    //represents a variable, constructed from Identifier token
    public class Variable : Node
    {
        public Token Token => _token;

        private Token _token;

        public Variable(Token token)
        {
            _token = token;
        }
    }
}

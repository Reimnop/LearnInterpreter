namespace LearnInterpreter
{
    public class TypeNode : Node
    {
        public Token Token => _token;

        private Token _token;

        public TypeNode(Token token)
        {
            _token = token;
        }
    }
}

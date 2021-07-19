namespace LearnInterpreter
{
    public class VariableDeclaration : Node
    {
        public Token Token => _token;
        public Node Assignment => _assign;

        private Token _token;
        private Node _assign;

        public VariableDeclaration(Token token, Node assign)
        {
            _token = token;
            _assign = assign;
        }
    }
}

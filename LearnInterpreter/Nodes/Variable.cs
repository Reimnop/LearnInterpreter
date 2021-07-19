namespace LearnInterpreter
{
    //represents a variable, constructed from Identifier token
    public class Variable : Node
    {
        public Token Token => _token;
        public Node IndexNode => _indexNode;

        private Token _token;
        private Node _indexNode;

        public Variable(Token token, Node indexNode)
        {
            _token = token;
            _indexNode = indexNode;
        }
    }
}

namespace LearnInterpreter
{
    public class ReturnStatement : Node
    {
        public Node Node => _node;

        private Node _node;

        public ReturnStatement(Node node)
        {
            _node = node;
        }
    }
}

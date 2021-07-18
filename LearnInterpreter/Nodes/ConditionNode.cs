namespace LearnInterpreter
{
    public class ConditionNode : Node
    {
        public Node Left => _left;
        public Node Right => _right;
        public Token Op => _op;

        private Node _left;
        private Node _right;
        private Token _op;

        public ConditionNode(Node left, Node right, Token op)
        {
            _left = left;
            _right = right;
            _op = op;
        }
    }
}

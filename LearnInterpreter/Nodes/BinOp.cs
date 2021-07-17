namespace LearnInterpreter
{
    public class BinOp : Node
    {
        public Token Token => _token;
        public Node Left => _left;
        public Node Right => _right;

        private Token _token;
        private Node _left;
        private Node _right;

        public BinOp(Token op, Node left, Node right)
        {
            _token = op;
            _left = left;
            _right = right;
        }
    }
}

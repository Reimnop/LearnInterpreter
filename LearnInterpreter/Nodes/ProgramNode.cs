namespace LearnInterpreter
{
    public class ProgramNode : Node
    {
        public Node Node => _node;

        private Node _node;

        public ProgramNode(Node node)
        {
            _node = node;
        }
    }
}

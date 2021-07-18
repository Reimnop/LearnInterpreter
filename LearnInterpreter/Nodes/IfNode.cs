namespace LearnInterpreter
{
    public class IfNode : Node
    {
        public BooleanNode Boolean => _boolean;
        public Block Body => _body;
        public int ScopeLevel;

        private BooleanNode _boolean;
        private Block _body;

        public IfNode(BooleanNode boolean, Block body)
        {
            _boolean = boolean;
            _body = body;
        }
    }
}

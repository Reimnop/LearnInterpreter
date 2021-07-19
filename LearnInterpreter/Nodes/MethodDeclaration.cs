namespace LearnInterpreter
{
    public class MethodDeclaration : Node
    {
        public string MethodName => _methodName;
        public Parameters Parameters => _parameters;
        public Block Block => _block;

        public MethodSymbol Symbol;

        private string _methodName;
        private Parameters _parameters;
        private Block _block;

        public MethodDeclaration(string name, Parameters parameters, Block block)
        {
            _methodName = name;
            _parameters = parameters;
            _block = block;
        }
    }
}

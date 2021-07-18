namespace LearnInterpreter
{
    public class VariableDeclaration : Node
    {
        public Variable Variable => _variable;
        public Node Assignment => _assign;

        private Variable _variable;
        private Node _assign;

        public VariableDeclaration(Variable var, Node assign)
        {
            _variable = var;
            _assign = assign;
        }
    }
}

namespace LearnInterpreter
{
    public class VariableDeclaration : Node
    {
        public TypeNode Type => _type;
        public Variable Variable => _variable;
        public Node Assignment => _assign;

        private TypeNode _type;
        private Variable _variable;
        private Node _assign;

        public VariableDeclaration(TypeNode type, Variable var, Node assign)
        {
            _type = type;
            _variable = var;
            _assign = assign;
        }
    }
}

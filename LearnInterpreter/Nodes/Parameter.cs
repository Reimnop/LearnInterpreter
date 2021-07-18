namespace LearnInterpreter
{
    public class Parameter : Node
    {
        public TypeNode Type => _type;
        public Variable Variable => _variable;

        private TypeNode _type;
        private Variable _variable;

        public Parameter(TypeNode type, Variable variable)
        {
            _type = type;
            _variable = variable;
        }
    }
}

namespace LearnInterpreter
{
    public class Parameter : Node
    {
        public Variable Variable => _variable;

        private Variable _variable;

        public Parameter(Variable variable)
        {
            _variable = variable;
        }
    }
}

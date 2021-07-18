using System.Collections.Generic;

namespace LearnInterpreter
{
    public class Parameters : Node
    {
        public List<Parameter> Children => _children;

        private List<Parameter> _children;

        public Parameters(List<Parameter> parameters)
        {
            _children = parameters;
        }
    }
}

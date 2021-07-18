using System.Collections.Generic;

namespace LearnInterpreter
{
    public class MethodCall : Node
    {
        public string MethodName => _methodName;
        public List<Node> Parameters => _parameters;
        public Token Token => _token;

        private string _methodName;
        private List<Node> _parameters;
        private Token _token;

        public MethodCall(string methodName, List<Node> parameters, Token token)
        {
            _methodName = methodName;
            _parameters = parameters;
            _token = token;
        }
    }
}

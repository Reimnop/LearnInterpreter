using System;
using System.Collections.Generic;
using System.Text;

namespace LearnInterpreter
{
    public class UnaryOp : Node
    {
        public Token Token => _token;
        public Node Expr => _expr;

        private Node _expr;
        private Token _token;

        public UnaryOp(Token token, Node expr)
        {
            _expr = expr;
            _token = token;
        }
    }
}

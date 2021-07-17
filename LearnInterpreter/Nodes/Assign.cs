using System;
using System.Collections.Generic;
using System.Text;

namespace LearnInterpreter
{
    //represents assignment "a = 42;"
    public class Assign : Node
    {
        public Token Token => _token;
        public Variable Left => _left;
        public Node Right => _right;

        private Token _token;
        private Variable _left;
        private Node _right;

        public Assign(Token token, Variable left, Node right)
        {
            _token = token;
            _left = left;
            _right = right;
        }
    }
}

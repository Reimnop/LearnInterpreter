using System;
using System.Collections.Generic;
using System.Text;

namespace LearnInterpreter
{
    //represents assignment "a = 42;"
    public class Assign : Node
    {
        public Token Token => _token;
        public Node Left => _left;
        public Node Right => _right;

        private Token _token;
        private Node _left;
        private Node _right;

        public Assign(Token token, Node left, Node right)
        {
            _token = token;
            _left = left;
            _right = right;
        }
    }
}

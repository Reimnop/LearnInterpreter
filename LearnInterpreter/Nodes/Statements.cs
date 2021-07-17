using System;
using System.Collections.Generic;
using System.Text;

namespace LearnInterpreter
{
    //represents a series of statements
    public class Statements : Node
    {
        public List<Node> Children;

        public Statements(List<Node> children)
        {
            Children = children;
        }
    }
}

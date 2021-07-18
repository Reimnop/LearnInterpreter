using System.Collections.Generic;

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

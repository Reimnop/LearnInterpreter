using System.Collections.Generic;

namespace LearnInterpreter
{
    public class ArrayNode : Node
    {
        public List<Node> Elements => _elements;

        private List<Node> _elements;

        public ArrayNode(List<Node> elements)
        {
            _elements = elements;
        }
    }
}

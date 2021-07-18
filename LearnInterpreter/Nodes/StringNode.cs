namespace LearnInterpreter
{
    public class StringNode : Node
    {
        public string Value => _value;

        private string _value;

        public StringNode(string value)
        {
            _value = value;
        }
    }
}

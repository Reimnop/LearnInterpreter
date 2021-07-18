namespace LearnInterpreter
{
    public abstract class Symbol
    {
        public string Name => _name;
        public int ScopeLevel;

        private string _name;

        public Symbol(string name)
        {
            _name = name;
        }
    }
}

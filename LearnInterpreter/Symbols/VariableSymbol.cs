namespace LearnInterpreter
{
    public class VariableSymbol : Symbol
    {
        public VariableSymbol(string name, BuiltinTypeSymbol type) : base(name, type)
        {
        }
    }
}

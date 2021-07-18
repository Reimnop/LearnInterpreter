namespace LearnInterpreter
{
    public class BuiltinMethodSymbol : Symbol
    {
        public delegate void BuiltinMethod(object[] parameters);

        public BuiltinMethod Method => _method;

        private BuiltinMethod _method;

        public BuiltinMethodSymbol(string name, BuiltinMethod method) : base(name)
        {
            _method = method;
        }
    }
}

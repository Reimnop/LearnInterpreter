using System.Collections.Generic;

namespace LearnInterpreter
{
    public class MethodSymbol : Symbol
    {
        public List<VariableSymbol> Parameters;

        public MethodSymbol(string name, List<VariableSymbol> parameters = null) : base(name)
        {
            Parameters = parameters ?? new List<VariableSymbol>();
        }
    }
}

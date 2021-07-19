using System.Collections.Generic;

namespace LearnInterpreter
{
    public class MethodSymbol : Symbol
    {
        public Block Body;
        public List<VariableSymbol> Parameters;
        public ActivationRecord SymbolRecord;

        public MethodSymbol(string name, Block body, List<VariableSymbol> parameters = null) : base(name)
        {
            Parameters = parameters ?? new List<VariableSymbol>();
            Body = body;
        }
    }
}

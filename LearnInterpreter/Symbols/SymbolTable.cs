using System;
using System.Collections.Generic;
using System.Text;

namespace LearnInterpreter
{
    public class SymbolTable
    {
        private Dictionary<string, Symbol> symbols = new Dictionary<string, Symbol>();

        public SymbolTable()
        {
            //init built in type symbols
            TryDefine(new BuiltinTypeSymbol("decimal"));
        }

        public bool TryDefine(Symbol symbol)
        {
            return symbols.TryAdd(symbol.Name, symbol);
        }

        public bool TryLookup(string name, out Symbol symbol)
        {
            return symbols.TryGetValue(name, out symbol);
        }

        public void Define(Symbol symbol)
        {
            symbols.Add(symbol.Name, symbol);
        }

        public Symbol Lookup(string name)
        {
            return symbols[name];
        }
    }
}

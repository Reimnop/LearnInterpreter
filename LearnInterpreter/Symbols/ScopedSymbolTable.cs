using System;
using System.Collections.Generic;

namespace LearnInterpreter
{
    public class ScopedSymbolTable
    {
        public ScopedSymbolTable EnclosingScope => _enclosingScope;
        public string ScopeName => _scopeName;
        public int ScopeLevel => _scopeLevel;

        private ScopedSymbolTable _enclosingScope;
        private string _scopeName;
        private int _scopeLevel;

        private Dictionary<string, Symbol> symbols = new Dictionary<string, Symbol>();

        public ScopedSymbolTable(string name, int level, ScopedSymbolTable enclosingScope = null)
        {
            _scopeName = name;
            _scopeLevel = level;
            _enclosingScope = enclosingScope;

            //init built in type symbols
            Define(new BuiltinTypeSymbol("decimal"));
        }

        public bool TryDefine(Symbol symbol)
        {
            return symbols.TryAdd(symbol.Name, symbol);
        }

        public bool TryLookup(string name, out Symbol symbol, bool currentScopeOnly = false)
        {
            if (currentScopeOnly)
            {
                return symbols.TryGetValue(name, out symbol);
            }

            if (symbols.TryGetValue(name, out symbol))
            {
                return true;
            }

            return _enclosingScope != null ? _enclosingScope.TryLookup(name, out symbol) : false;
        }

        public void Define(Symbol symbol)
        {
            symbols.Add(symbol.Name, symbol);
        }

        public Symbol Lookup(string name, bool currentScopeOnly = false)
        {
            if (currentScopeOnly)
            {
                return symbols[name];
            }

            if (TryLookup(name, out Symbol symbol))
            {
                return symbol;
            }

            return _enclosingScope != null ? _enclosingScope.Lookup(name) : throw new Exception($"The name \"{name}\" does not exist in the current context!");
        }
    }
}

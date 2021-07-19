using System;
using System.Collections.Generic;

namespace LearnInterpreter
{
    public class ScopedSymbolTable
    {
        public ScopedSymbolTable EnclosingScope => _enclosingScope;
        public int ScopeLevel => _scopeLevel;

        private ScopedSymbolTable _enclosingScope;
        private int _scopeLevel;

        private Dictionary<string, Symbol> symbols = new Dictionary<string, Symbol>();

        public ScopedSymbolTable(int level, ScopedSymbolTable enclosingScope = null)
        {
            _scopeLevel = level;
            _enclosingScope = enclosingScope;

            Define(new BuiltinMethodSymbol("print", Builtin_Print));
            Define(new BuiltinMethodSymbol("println", Builtin_PrintLine));
            Define(new BuiltinMethodSymbol("arrayAppend", Builtin_ArrayAppend));
        }

        private void Builtin_Print(object[] parameters)
        {
            foreach (object param in parameters)
            {
                Console.Write(param);
            }
        }

        private void Builtin_PrintLine(object[] parameters)
        {
            foreach (object param in parameters)
            {
                Console.Write(param);
            }
            Console.WriteLine();
        }

        private void Builtin_ArrayAppend(object[] parameters)
        {
            if (parameters.Length != 2)
            {
                throw new Exception("Invalid parameters!");
            }

            if (parameters[0] is List<dynamic>)
            {
                List<dynamic> list = (List<dynamic>)parameters[0];
                list.Add(parameters[1]);
                return;
            }

            throw new Exception("Invalid parameters!");
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
            symbol.ScopeLevel = _scopeLevel;
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

        public override string ToString()
        {
            const int align = 10;

            string s = 
                $"SCOPED SYMBOL TABLE{Environment.NewLine}" +
                $"Level: {_scopeLevel}{Environment.NewLine}" +
                $"Enclosing scope: {(_enclosingScope != null ? _enclosingScope.ScopeLevel.ToString() : "None")}{Environment.NewLine}" +
                $"================================{Environment.NewLine}";
            foreach (Symbol symbol in symbols.Values)
            {
                s += symbol.Name;
                for (int i = 0; i < align - symbol.Name.Length; i++)
                {
                    s += " ";
                }
                s += $" : {(symbol is MethodSymbol ? "Method" : "Variable")}{Environment.NewLine}";
            }

            return s;
        }
    }
}

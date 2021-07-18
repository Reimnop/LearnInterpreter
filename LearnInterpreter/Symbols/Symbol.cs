﻿namespace LearnInterpreter
{
    public abstract class Symbol
    {
        public string Name => _name;
        public Symbol Type => _type;
        public int ScopeLevel;

        private string _name;
        private Symbol _type;

        public Symbol(string name, Symbol type)
        {
            _name = name;
            _type = type;
        }

        public Symbol(string name)
        {
            _name = name;
            _type = null;
        }
    }
}

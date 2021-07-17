using System;
using System.Collections.Generic;
using System.Text;

namespace LearnInterpreter
{
    public class VariableSymbol : Symbol
    {
        public VariableSymbol(string name, Symbol type) : base(name, type)
        {
        }
    }
}

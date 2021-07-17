using System;
using System.Collections.Generic;
using System.Text;

namespace LearnInterpreter
{
    public class Declaration : Node
    {
        public TypeNode Type => _type;
        public Variable Variable => _variable;

        private TypeNode _type;
        private Variable _variable;

        public Declaration(TypeNode type, Variable var)
        {
            _type = type;
            _variable = var;
        }
    }
}

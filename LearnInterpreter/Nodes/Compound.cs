using System;
using System.Collections.Generic;
using System.Text;

namespace LearnInterpreter
{
    //represents a block "{ ... }"
    public class Compound : Node
    {
        public Statements Statements => _statements;

        private Statements _statements;

        public Compound(Statements statements)
        {
            _statements = statements;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace LearnInterpreter
{
    //represents a block "{ ... }"
    public class Block : Node
    {
        public Statements Statements => _statements;

        private Statements _statements;

        public Block(Statements statements)
        {
            _statements = statements;
        }
    }
}

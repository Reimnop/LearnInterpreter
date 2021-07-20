using System;
using System.Collections.Generic;
using System.Text;

namespace LearnInterpreter
{
    public class WhileLoop : Node
    {
        public BooleanNode Boolean => _boolean;
        public Block Body => _body;
        public int ScopeLevel;

        private BooleanNode _boolean;
        private Block _body;

        public WhileLoop(BooleanNode boolean, Block body)
        {
            _boolean = boolean;
            _body = body;
        }
    }
}

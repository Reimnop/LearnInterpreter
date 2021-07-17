using System;
using System.Collections.Generic;
using System.Text;

namespace LearnInterpreter
{
    public class MethodDeclaration : Node
    {
        public string MethodName => _methodName;
        public Block Block => _block;

        private string _methodName;
        private Block _block;

        public MethodDeclaration(string name, Block block)
        {
            _methodName = name;
            _block = block;
        }
    }
}

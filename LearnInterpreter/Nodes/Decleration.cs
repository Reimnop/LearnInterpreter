using System;
using System.Collections.Generic;
using System.Text;

namespace LearnInterpreter
{
    public class Decleration : Node
    {
        public Token TypeToken => _type;
        public Token IdentifierToken => _id;

        private Token _type;
        private Token _id;

        public Decleration(Token type, Token id)
        {
            _type = type;
            _id = id;
        }
    }
}

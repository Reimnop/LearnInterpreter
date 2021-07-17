using System;
using System.Collections.Generic;
using System.Text;

namespace LearnInterpreter
{
    public class Number : Node
    {
        public string Value => _value;

        private string _value;

        public Number(string value)
        {
            _value = value;
        }
    }
}

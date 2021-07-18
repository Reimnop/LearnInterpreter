using System;

namespace LearnInterpreter
{
    public class Error : Exception
    {
        public string ErrorCode => _errorCode;
        public Token Token => _token;

        private string _errorCode;
        private Token _token;

        public Error(string errorCode, Token token, string message) : base(message)
        {
            _errorCode = errorCode;
            _token = token;
        }
    }
}

namespace LearnInterpreter
{
    public class ParserError : Error
    {
        public ParserError(string errorCode, Token token, string message) : base(errorCode, token, message)
        {
        }
    }
}

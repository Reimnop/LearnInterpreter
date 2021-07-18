namespace LearnInterpreter
{
    public class LexerError : Error
    {
        public LexerError(string errorCode, Token token, string message) : base(errorCode, token, message)
        {
        }
    }
}

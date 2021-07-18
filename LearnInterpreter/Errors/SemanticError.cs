namespace LearnInterpreter
{
    public class SemanticError : Error
    {
        public SemanticError(string errorCode, Token token, string message) : base(errorCode, token, message)
        {

        }
    }
}

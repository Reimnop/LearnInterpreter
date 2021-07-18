namespace LearnInterpreter
{
    public class ErrorCodes
    {
        //avoid creating new instances
        private ErrorCodes() { }

        public const string
            UnexpectedToken = "Unexpected token",
            IdentifierNotFound = "Identifier not found",
            DuplicateIdentifier = "Duplicate identifier",
            ParamCountMismatch = "Incorrect parameter count";
    }
}

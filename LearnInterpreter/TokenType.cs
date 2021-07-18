namespace LearnInterpreter
{
    public enum TokenType
    {
        Plus         = '+',
        Minus        = '-',
        Mult         = '*',
        Div          = '/',
        LeftParen    = '(',
        RightParen   = ')',
        OpenBracket  = '{',
        CloseBracket = '}',
        Assign       = '=',
        Semicolon    = ';',
        Dot          = '.',
        Comma        = ',',
        Eof          = 0,
        Integer      = 1,
        Identifier   = 2,
        Type         = 3,
        Void         = 4
    }
}

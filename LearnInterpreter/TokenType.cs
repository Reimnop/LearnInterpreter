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
        LessThan     = '<',
        GreaterThan  = '>',
        Eof          = 0,
        Integer      = 1,
        Identifier   = 2,
        Var         = 3,
        Void         = 4,
        If           = 5,
        True         = 6,
        False        = 7,
        Equal        = 8,
        String       = 9
    }
}

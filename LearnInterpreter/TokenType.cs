namespace LearnInterpreter
{
    public enum TokenType
    {
        Plus                 = '+',
        Minus                = '-',
        Mult                 = '*',
        Div                  = '/',
        LeftParen            = '(',
        RightParen           = ')',
        OpenBracket          = '{',
        CloseBracket         = '}',
        Assign               = '=',
        Semicolon            = ';',
        Dot                  = '.',
        Comma                = ',',
        LessThan             = '<',
        GreaterThan          = '>',
        LeftSquareBracket    = '[',
        RightSquareBracket   = ']',
        Eof                  = 0,
        Integer              = 1,
        Identifier           = 2,
        Var                  = 3,
        Function             = 4,
        If                   = 5,
        True                 = 6,
        False                = 7,
        Equal                = 8,
        NotEqual             = 9,
        LessThanOrEqualTo    = 10,
        GreaterThanOrEqualTo = 11,
        String               = 12,
        Return               = 13,
        For                  = 14,
        While                = 15
    }
}

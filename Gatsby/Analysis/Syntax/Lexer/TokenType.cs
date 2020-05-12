namespace Gatsby.Analysis.Syntax.Lexer
{
    public enum TokenType
    {
        // Tokens
        Bad,
        EndOfFile,
        Whitespace,
        Assign,
        Number,
        Plus,
        Minus,
        Star,
        Slash,
        ReverseSlash,
        Modulo,
        LogicalAnd,
        LogicalOr,
        Negation,
        Power,
        OpenParenthesis,
        CloseParenthesis,
        Identifier,
        Equality,
        NotEqualsTo,
        LessThan,
        LessThanEquals,
        GreaterThan,
        GreaterThanEquals,
        BitwiseAnd,
        BitwiseOr,
        BitwiseXor,
        BitwiseNegation,
        LeftShift,
        RightShift,

        // Expressions
        LiteralExpression,
        UnaryExpression,
        BinaryExpression,
        ParenthesizedExpression,
        NameExpression,
        AssignmentExpression,

        //Keywords
        TrueKeyword,
        FalseKeyword
    }
}
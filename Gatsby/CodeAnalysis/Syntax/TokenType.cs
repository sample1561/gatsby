namespace Gatsby.CodeAnalysis.Syntax
{
    public enum TokenType
    {
        // Tokens
        Bad,
        EndOfFile,
        Whitespace,
        Number,
        Plus,
        Minus,
        Star,
        Slash,
        Modulo,
        Power,
        OpenParenthesis,
        CloseParenthesis,
        Identifier,

        // Expressions
        LiteralExpression,
        UnaryExpression,
        BinaryExpression,
        ParenthesizedExpression,

        //Keywords
        TrueKeyword,
        FalseKeyword
    }
}
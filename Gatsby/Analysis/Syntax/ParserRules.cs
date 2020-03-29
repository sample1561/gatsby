namespace Gatsby.Analysis.Syntax
{
    internal static class ParserRules
    {
        public static int GetUnaryOperatorPrecedence(this TokenType kind)
        {
            return kind switch
            {
                TokenType.Plus => 5,
                TokenType.Minus => 5,
                TokenType.Negation => 5,
                _ => 0
            };
        }

        public static int GetBinaryOperatorPrecedence(this TokenType kind)
        {
            return kind switch
            {
                TokenType.LogicalAnd => 1,
                TokenType.LogicalOr => 1,
                TokenType.Plus => 2,
                TokenType.Minus => 2,
                TokenType.Star => 3,
                TokenType.Slash => 3,
                TokenType.Modulo => 3,
                TokenType.Power => 4,
                _ => 0
            };
        }

        public static TokenType GetKeywordKind(string text)
        {
            text = text.ToLower();

            return text switch
            {
                "true" => TokenType.TrueKeyword,
                "false" => TokenType.FalseKeyword,
                _ => TokenType.Identifier
            };
        }
    }
}
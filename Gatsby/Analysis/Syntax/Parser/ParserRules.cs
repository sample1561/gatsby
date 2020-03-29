using Gatsby.Analysis.Syntax.Lexer;

namespace Gatsby.Analysis.Syntax.Parser
{
    internal static class ParserRules
    {
        public static int GetUnaryOperatorPrecedence(this TokenType kind)
        {
            return kind switch
            {
                TokenType.Plus => 6,
                TokenType.Minus => 6,
                TokenType.Negation => 6,
                TokenType.BitwiseNegation => 6,
                _ => 0
            };
        }

        public static int GetBinaryOperatorPrecedence(this TokenType kind)
        {
            return kind switch
            {
                TokenType.LogicalAnd => 1,
                TokenType.LogicalOr => 1,
                
                TokenType.BitwiseAnd => 2,
                TokenType.BitwiseOr => 2,
                TokenType.BitwiseXor => 2,
                TokenType.LeftShift => 2,
                TokenType.RightShift => 2,

                TokenType.Plus => 3,
                TokenType.Minus => 3,
                
                TokenType.EqualsTo => 4,
                TokenType.NotEqualsTo => 4,
                TokenType.GreaterThan => 4,
                TokenType.GreaterThanEquals => 4,
                TokenType.LessThan => 4,
                TokenType.LessThanEquals => 4,
                
                TokenType.Star => 5,
                TokenType.ReverseSlash => 5,
                TokenType.Slash => 5,
                TokenType.Modulo => 5,
                
                TokenType.Power => 6,
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
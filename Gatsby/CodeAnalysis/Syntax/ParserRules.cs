using System;

namespace Gatsby.CodeAnalysis.Syntax
{
    internal static class ParserRules
    {
        public static int GetUnaryOperatorPrecedence(this TokenType kind)
        {
            switch (kind)
            {
                case TokenType.Plus:
                case TokenType.Minus:
                    return 5;
                
                case TokenType.Negation:
                    return 5;

                default:
                    return 0;
            }
        }

        public static int GetBinaryOperatorPrecedence(this TokenType kind)
        {
            switch (kind)
            {
                case TokenType.And:
                case TokenType.Or:
                    return 1;
                
                case TokenType.Plus:
                case TokenType.Minus:
                    return 2;
                
                case TokenType.Star:
                case TokenType.Slash:
                case TokenType.Modulo:
                    return 3;
                
                case TokenType.Power:
                    return 4;

                

                default:
                    return 0;
            }
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